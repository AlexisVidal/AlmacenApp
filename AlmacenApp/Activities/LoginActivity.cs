using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AlmacenApp.Clases;
using AlmacenApp.Models;
using AlmacenApp.Resources.DataHelpers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AlmacenApp.Fragments
{
    [Activity(Label = "ErpApp", MainLauncher = true, WindowSoftInputMode = SoftInput.AdjustPan, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, Theme = "@style/AppFullScreenTheme")]
    public class LoginActivity : Activity
    {
        public class Usuario
        {
            public string IDUSUARIO { get; set; }
            public string PASSWORD { get; set; }
        }
        private readonly Context _mContext = Application.Context;
        EditText _edtUser, _edtPassword;
        TextInputLayout _txtUser, _txtPassword;
        Spinner _spinner_empresa;
        Button _btnLogin;
        string _empresa = "";
        ProgressDialog progress;
        string _claveEncrip = "";

        UsuarioErp _usuario = null;
        List<UsuarioErp> _lusuario = null;
        private AppPreferences _ap;
        public static string serverurl = "";
        public static string ruc = "";
        public static string nombreempre = "";
        public static string direccionempre = "";
        DataHelpers db;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.login_layout);
            _edtUser = FindViewById<EditText>(Resource.Id.edt_user);
            _edtPassword = FindViewById<EditText>(Resource.Id.edt_password);
            _txtUser = FindViewById<TextInputLayout>(Resource.Id.txt_user);
            _txtPassword = FindViewById<TextInputLayout>(Resource.Id.txt_password);
            _spinner_empresa = FindViewById<Spinner>(Resource.Id.spinner_empresa);
            _spinner_empresa.ItemSelected += _spinner_empresa_ItemSelected;
            _btnLogin = FindViewById<Button>(Resource.Id.btn_login);
            _btnLogin.Click += _btnLogin_Click;

            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.empresa_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            _spinner_empresa.Adapter = adapter;

            _ap = new AppPreferences(_mContext);
        }

        private void _btnLogin_Click(object sender, EventArgs e)
        {
            progress = new ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            progress.SetMessage("Validando...");
            progress.SetCancelable(false);
            RunOnUiThread(() =>
            {
                progress.Show();
            });
            if (!ValidaUsuario())
            {
                Toast.MakeText(this, "Campo usuario esta vacio!", ToastLength.Short).Show();
                if (progress != null)
                {
                    progress.Hide();
                }
                return;
            }
            if (!ValidaContrasena())
            {
                Toast.MakeText(this, "Campo contraseña esta vacio!", ToastLength.Short).Show();
                if (progress != null)
                {
                    progress.Hide();
                }
                return;
            }
            if (_empresa==""|| _empresa=="...")
            {
                Toast.MakeText(this, "Debe seleccionar empresa valida!", ToastLength.Short).Show();
                if (progress != null)
                {
                    progress.Hide();
                }
                return;
            }
            else if (_empresa == "Vilocrusac")
            {
                serverurl = GetString(Resource.String.vilocruserver);
                ruc = GetString(Resource.String.vilocruruc);
                nombreempre = GetString(Resource.String.vilocruempresa);
                direccionempre = GetString(Resource.String.vilocrudireccion);
            }
            else if (_empresa == "Corcrusac")
            {
                serverurl = GetString(Resource.String.corcruserver);
                ruc = GetString(Resource.String.corcruruc);
                nombreempre = GetString(Resource.String.corcruempresa);
                direccionempre = GetString(Resource.String.corcrudireccion);
            }
            string usuario = _edtUser.Text;
            string password = _edtPassword.Text;
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] inputBytes = (new System.Text.UnicodeEncoding()).GetBytes(password);
            byte[] hash = sha1.ComputeHash(inputBytes);
            _claveEncrip = Convert.ToBase64String(hash);
            

            var w = Task.Run(async () =>
            {
                var info = await ValidaLogueo(usuario, _claveEncrip);
                if (info.IsSuccessStatusCode && info.RequestMessage != null)
                {
                    JArray usuarioe = JArray.Parse(await info.Content.ReadAsStringAsync());
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    if (usuarioe != null && usuarioe.Count > 0)
                    {
                        _lusuario = JsonConvert.DeserializeObject<List<UsuarioErp>>(usuarioe.ToString(), settings);

                        if (_lusuario != null && _lusuario.Count > 0)
                        {
                            UsuarioErp usuarioen = new UsuarioErp();
                            usuarioen = _lusuario.FirstOrDefault();
                            _ap.saveAccessKey(usuarioen.IDUSUARIO.ToString());
                            _ap.saveIdKey(usuarioen.IDUSUARIO.ToString());
                            _ap.saveNombresKey(usuarioen.NOMBRES + " " + usuarioen.A_PATERNO + " " + usuarioen.A_MATERNO);
                            _ap.saveServerurlKey(serverurl);
                            _ap.saveRucKey(ruc);
                            _ap.saveEmpresaKey(nombreempre);
                            _ap.saveDireccionKey(direccionempre);
                            _ap.saveServerurlKey(serverurl);
                            var newact = new Intent(this, typeof(MainActivity));
                            try
                            {
                                StartActivity(newact);
                                Finish();
                            }
                            catch (Exception az)
                            {
                                RunOnUiThread(() => Toast.MakeText(this, "Exception en OnCreate(): " + az.Message, ToastLength.Short).Show());
                            }
                        }
                        else
                        {
                            RunOnUiThread(() => Toast.MakeText(this, "Imposible Conectarse al servidor!", ToastLength.Short).Show());
                        }
                    }
                    else
                    {
                        RunOnUiThread(() => Toast.MakeText(this, "Credenciales Incorrectas!", ToastLength.Short).Show());
                    }
                }
            }).ContinueWith(info => RunOnUiThread(() => progress.Hide()));
        }

        private async Task<HttpResponseMessage> ValidaLogueo(string usuario, string claveEncrip)
        {
            try
            {
                Usuario entidad = new Usuario { IDUSUARIO = usuario, PASSWORD = claveEncrip };
                HttpClient client = new HttpClient();
                var connectionInfo = serverurl;
                client.BaseAddress = new Uri(connectionInfo);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string serializedObject = JsonConvert.SerializeObject(entidad);
                HttpContent contentPost = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Usuario/login", contentPost);
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private bool ValidaUsuario()
        {
            if (_edtUser.Length() == 0)
            {
                _txtUser.ErrorEnabled = true;
                _txtUser.SetErrorTextAppearance(Resource.String.error_user);
                return false;
            }
            _txtUser.ErrorEnabled = false;
            return true;
        }
        private bool ValidaContrasena()
        {
            if (_edtPassword.Length() == 0)
            {
                _txtPassword.ErrorEnabled = true;
                _txtPassword.SetErrorTextAppearance(Resource.String.error_password);
                return false;
            }
            _txtPassword.ErrorEnabled = false;
            return true;
        }
        private void _spinner_empresa_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner _spinnerEmpre = (Spinner)sender;
            //string toast = string.Format("Seleccionado es {0}", _spinnerEmpre.GetItemAtPosition(e.Position));
            //Toast.MakeText(this, toast, ToastLength.Long).Show();
            _empresa = _spinnerEmpre.GetItemAtPosition(e.Position).ToString().Trim();

        }
    }
}