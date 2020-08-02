using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Mvc;
using WebSite.Models;
using static System.Collections.Specialized.BitVector32;

namespace WebSite.Controllers
{
    public class HomeController : Controller
    {
        private HttpClient client = null;
        private Login login = null;
        private static string usuario;
        private static string senha;
        
        public ActionResult Index()
        {
            return View("Login");
        }

        public ActionResult Login(Login _login)
        {
            if (RequestLogin(_login, "http://localhost/APIService/v1/Cadastro/Login"))
            {
                this.login = _login;
                this.login.SetIsValid(true);
                usuario = _login.USUARIO;
                senha = _login.SENHA;
                return View("Selecao");
            }
            else
            {
                this.login = new Login();
                this.login.SetIsValid(false);
                usuario = string.Empty;
                senha = string.Empty;
                return View("Login");
            }
        }

        public ActionResult Selecao()
        {
            if (!string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(senha))
                return View("Selecao");
            else
                return View("Login");
        }

        public ActionResult Deslogar()
        {
            this.login = new Login();
            this.login.SetIsValid(false);
            usuario = string.Empty;
            senha = string.Empty;
            return View("Login");
        }

        public ActionResult Registrar()
        {
            if (!string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(senha))
            {
                IEnumerable<string> generoParm = new List<string> { "Masculino", "Feminino", "Outro", "Nao_Declarar" };
                ViewBag.Genero = new SelectList(generoParm, 0);
                return View();
            }
            else
            {
                this.login = new Login();
                this.login.SetIsValid(false);
                usuario = string.Empty;
                senha = string.Empty;
                return View("Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registro(Cadastro _cadastro)
        {
            if (RegistrarCadastro(_cadastro, "http://localhost/APIService/v1/Cadastro"))
                ViewBag.RegistroEfetuado = true;
            else
                ViewBag.RegistroEfetuado = false;

            return View();
        }

        public ActionResult Atualizar()
        {
            if (!string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(senha))
            {
                IEnumerable<string> generoParm = new List<string> { "Masculino", "Feminino", "Outro", "Nao_Declarar" };
                ViewBag.Genero = new SelectList(generoParm, 0);
                return View();
            }
            else
            {
                this.login = new Login();
                this.login.SetIsValid(false);
                usuario = string.Empty;
                senha = string.Empty;
                return View("Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualiza(Cadastro _cadastro)
        {
            if (AtualizaCadastro(_cadastro, "http://localhost/APIService/v1/Cadastro"))
                ViewBag.RegistroAtualizado = true;
            else
                ViewBag.RegistroAtualizado = false;

            return View();
        }
        
        public ActionResult Buscar()
        {
            if (!string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(senha))
                return View();
            else
            {
                this.login = new Login();
                this.login.SetIsValid(false);
                usuario = string.Empty;
                senha = string.Empty;
                return View("Login");
            }
        }

        [HttpGet]
        public ActionResult Busca(Cadastro _cadastro)
        {
            Cadastro prmView = BuscaCadastro(_cadastro, $"http://localhost/APIService/v1/Cadastro/{_cadastro.CPF}");
            if (prmView != null)
                return View(prmView);
            else
                return View();
        }
        
        public ActionResult Deletar()
        {
            if (!string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(senha))
                return View();
            else
            {
                this.login = new Login();
                this.login.SetIsValid(false);
                usuario = string.Empty;
                senha = string.Empty;
                return View("Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deleta(Cadastro _cadastro)
        {
            if (DeletaCadastro($"http://localhost/APIService/v1/Cadastro/{_cadastro.CPF}"))
                ViewBag.RegistroExcluido = true;
            else
                ViewBag.RegistroExcluido = false;
            return View();
        }
        
        private bool RequestLogin(object prm, string endpoint)
        {
            try
            {
                this.client = new HttpClient();
                //this.client.BaseAddress = new Uri(endpoint);
                this.login = (Login)prm;
                this.login.SetIsValid(true);

                if (!AdicionarHeadersLogin())
                {
                    this.login.SetIsValid(false);
                    usuario = string.Empty;
                    senha = string.Empty;
                    return false;
                }

                HttpResponseMessage response = client.GetAsync(endpoint).Result;

                if (response.IsSuccessStatusCode)
                    return true;
                else
                {
                    usuario = string.Empty;
                    senha = string.Empty;
                    return false;
                }   
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool RegistrarCadastro(object prm, string endpoint)
        {
            try
            {
                this.client = new HttpClient();
                this.client.BaseAddress = new Uri(endpoint);

                if (!AdicionarHeaders())
                    return false;

                var json = JsonConvert.SerializeObject(prm);
                var contString = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync(endpoint, contString).Result;

                if (response.StatusCode == HttpStatusCode.Created)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool AtualizaCadastro(object prm, string endpoint)
        {
            try
            {
                this.client = new HttpClient();
                this.client.BaseAddress = new Uri(endpoint);

                if (!AdicionarHeaders())
                    return false;

                var json = JsonConvert.SerializeObject(prm);
                var contString = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PutAsync(endpoint, contString).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Cadastro BuscaCadastro(Cadastro prm, string endpoint)
        {
            try
            {
                this.client = new HttpClient();
                this.client.BaseAddress = new Uri(endpoint);

                if (!AdicionarHeaders())
                    return null;

                HttpResponseMessage response = client.GetAsync(endpoint).Result;
                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                if (response.StatusCode == HttpStatusCode.OK)
                    return JsonConvert.DeserializeObject<Cadastro>(json);
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool DeletaCadastro(string endpoint)
        {
            try
            {
                this.client = new HttpClient();
                this.client.BaseAddress = new Uri(endpoint);

                if (!AdicionarHeaders())
                    return false;

                HttpResponseMessage response = client.DeleteAsync(endpoint).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool AdicionarHeaders()
        {
            if (string.IsNullOrEmpty(usuario) && string.IsNullOrEmpty(senha))
                return false;
            
            this.client.DefaultRequestHeaders.Accept.Clear();
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this.client.DefaultRequestHeaders.Add("login", usuario);
            this.client.DefaultRequestHeaders.Add("password", senha);

            return true;
        }

        private bool AdicionarHeadersLogin()
        {
            if (this.login == null)
                return false;

            this.client.DefaultRequestHeaders.Accept.Clear();
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this.client.DefaultRequestHeaders.Add("login", this.login.USUARIO);
            this.client.DefaultRequestHeaders.Add("password", this.login.SENHA);

            return true;
        }
    }
}