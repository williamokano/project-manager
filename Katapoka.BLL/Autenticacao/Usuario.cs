using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Katapoka.BLL.Autenticacao
{
    public class Usuario
    {
        public string DsNome { get; set; }
        public string DsHashLogin { get; set; }
        public int IdUsuario { get; set; }

        internal static Usuario CriaUsuario(Katapoka.DAO.Usuario_Tb usuario)
        {
            if (usuario == null)
                return null;
            return new Usuario()
            {
                DsNome = usuario.DsNome,
                IdUsuario = usuario.IdUsuario,
                DsHashLogin = usuario.DsHashLogin
            };
        }

        public static Usuario UsuarioAtual
        {
            get
            {
                if (System.Web.HttpContext.Current == null)
                    return null;

                if (System.Web.HttpContext.Current
                    .Request.Cookies["userLogin"] == null)
                    return null;

                string hashLogin = System.Web.HttpContext.Current
                    .Request.Cookies["userLogin"].Value.ToString();
                using (Katapoka.BLL.Usuario.UsuarioBLL usuarioBLL =
                    new BLL.Usuario.UsuarioBLL())
                {
                    Katapoka.DAO.Usuario_Tb usuario = usuarioBLL.GetByHashLogin(hashLogin);

                    if (usuario == null)
                        return null;

                    if (usuario.DtValidadeLogin == null || usuario.DtValidadeLogin.Value < DateTime.Now)
                        return null;

                    //Update expires times
                    usuario.DtValidadeLogin = DateTime.Now.AddMinutes(Constantes.TempoValidadeLogin);
                    usuarioBLL.Save(usuario);

                    //Update cookie login
                    HttpCookie cookieLogin = HttpContext.Current.Request.Cookies["userLogin"];
                    cookieLogin.Expires = usuario.DtValidadeLogin.Value;
                    HttpContext.Current.Response.Cookies.Add(cookieLogin);

                    return CriaUsuario(usuario);
                }
            }
        }

        public static void Logout()
        {
            using (Katapoka.BLL.Usuario.UsuarioBLL usuarioBLL = new BLL.Usuario.UsuarioBLL())
            {
                if (System.Web.HttpContext.Current != null
                    && System.Web.HttpContext.Current.Request.Cookies["userLogin"] != null)
                {
                    string hashLogin = System.Web.HttpContext
                        .Current.Request.Cookies["userLogin"].Value.ToString();
                    if (!string.IsNullOrWhiteSpace(hashLogin))
                    {
                        Katapoka.DAO.Usuario_Tb usuario = usuarioBLL.GetByHashLogin(hashLogin);
                        if (usuario != null)
                        {
                            usuario.DtValidadeLogin = DateTime.Now.AddDays(-1);
                            usuario.DsHashLogin = Guid.NewGuid().ToString("N").ToUpper();
                            usuarioBLL.Save(usuario);
                        }
                    }
                    System.Web.HttpContext.Current.Response.Cookies["userLogin"].Expires = DateTime.Now.AddDays(-1);
                }
            }
        }

        public static void Login(string email, string senha)
        {
            using (Katapoka.BLL.Usuario.UsuarioBLL usuarioBLL = new BLL.Usuario.UsuarioBLL())
            {
                Katapoka.DAO.Usuario_Tb usuario = usuarioBLL.GetByEmail(email);
                if (usuario == null)
                    throw new Exception("Usuário não encontrado com o e-mail fornecido.");

                if (usuario.DsSenha != Katapoka.BLL.Utilitarios.Criptografia.GetHash(senha))
                    throw new Exception("Senha incorreta.");

                if (System.Web.HttpContext.Current == null)
                    throw new Exception("This method should be used only in web applications");

                string calculatedHash = Guid.NewGuid().ToString("N").ToUpper();
                System.Web.HttpCookie loginCookie = new HttpCookie("userLogin");
                loginCookie.Value = calculatedHash;
                loginCookie.Expires = DateTime.Now.AddMinutes(Constantes.TempoValidadeLogin);
                System.Web.HttpContext.Current.Response.Cookies.Add(loginCookie);

                //Cria cookie login
                usuario.DsHashLogin = calculatedHash;
                usuario.DtValidadeLogin = DateTime.Now.AddMinutes(Constantes.TempoValidadeLogin);
                usuarioBLL.Save(usuario);
            }
        }

    }
}
