using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Katapoka.DAO;

/// <summary>
/// Summary description for API
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class API : System.Web.Services.WebService {

    public API () {

    }

    /// <summary>
    /// Método que recupera todas as cidades de um determinado estado
    /// </summary>
    /// <param name="CdUF">Sigla do estado</param>
    /// <returns>Lista das cidades do estado cotnendo ID, Nome da Cidade e sigla do estado</returns>
    [WebMethod(true)]
    [System.Web.Script.Services.ScriptMethod(UseHttpGet=false, ResponseFormat=System.Web.Script.Services.ResponseFormat.Json)]
    public List<Katapoka.DAO.Regiao.Cidade> GetCidades(string CdUF) {
        using (Katapoka.BLL.Regiao.CidadeBLL cidadeBLL = new Katapoka.BLL.Regiao.CidadeBLL())
        {
            return cidadeBLL.GetAll(CdUF)
                .Select(p => new Katapoka.DAO.Regiao.Cidade()
                {
                    CdUF = p.CdUF,
                    DsNome = p.DsNome,
                    IdCidade = p.IdCidade
                }).ToList();
        }
    }

    /// <summary>
    /// Recupera os bairros de uma determinada cidade
    /// </summary>
    /// <param name="idCidade">id da cidade de onde quer recueprar os bairros</param>
    /// <returns>lista de bairros contendo id do bairro, nome e id da cidade</returns>
    [WebMethod(true)]
    [System.Web.Script.Services.ScriptMethod(UseHttpGet = false, ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public List<Katapoka.DAO.Regiao.Bairro> GetBairros(int IdCidade)
    {
        using (Katapoka.BLL.Regiao.BairroBLL bairroBLL = new Katapoka.BLL.Regiao.BairroBLL())
        {
            return bairroBLL.GetAll(IdCidade)
                .Select(p => new Katapoka.DAO.Regiao.Bairro()
                {
                    DsNome = p.DsNome,
                    IdBairro = p.IdBairro,
                    IdCidade = p.IdCidade
                }).ToList();
        }
    }

    [WebMethod(true)]
    [System.Web.Script.Services.ScriptMethod(UseHttpGet = false, ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public Katapoka.DAO.JsonResponse  GetUsuarioAutocomplete(string nome)
    {
        Katapoka.DAO.JsonResponse response = new Katapoka.DAO.JsonResponse();
        try
        {
            using (Katapoka.BLL.Usuario.UsuarioBLL usuarioBLL = new Katapoka.BLL.Usuario.UsuarioBLL())
            {
                response.Status = 200;
                response.Data = usuarioBLL.GetByNome(nome, Katapoka.BLL.Usuario.EsquemaBuscaNome.ComecandoCom)
                    .Select(p => new Katapoka.DAO.UsuarioCompleto()
                    {
                        DsNome = p.DsNome,
                        IdUsuario = p.IdUsuario
                    }).ToList();
            }
        }
        catch (Exception ex)
        {
            response.Status = 500;
            response.Data = ex.Message;
        }
        return response;
    }

    [WebMethod(true)]
    [System.Web.Script.Services.ScriptMethod(UseHttpGet = false, ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public List<Katapoka.DAO.Atividade.AtividadeCompleta> GetAtividadesByProjeto(int idProjeto)
    {
        using (Katapoka.BLL.Atividade.AtividadeBLL atividadeBLL = new Katapoka.BLL.Atividade.AtividadeBLL())
        {
            return atividadeBLL.GetFiltroQueryAtividades(null, idProjeto, null, null, null, null, null, Katapoka.BLL.Atividade.AtividadeBLL.OrdenacaoAtividade.IdC)
                .Select(p => new Katapoka.DAO.Atividade.AtividadeCompleta()
                {
                    DsNomeAtividade = p.DsTituloAtividade,
                    IdAtividade = p.IdAtividade,
                    IdAtividadeLocal = 0,
                    IdPreAtividade = p.IdAtividadePredecessora
                }).ToList();
        }
    }

    [WebMethod(true)]
    [System.Web.Script.Services.ScriptMethod(UseHttpGet = false, ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public List<Katapoka.DAO.Projeto.ProjetoCompleto> GetProjetosFiltroAtivo(bool flProjetoAtivo)
    {
        using (Katapoka.BLL.Projeto.ProjetoBLL projetoBLL = new Katapoka.BLL.Projeto.ProjetoBLL())
        {
            return projetoBLL.GetAll(flProjetoAtivo)
                .Select(p => new Katapoka.DAO.Projeto.ProjetoCompleto()
                {
                    DsCodigoReferencia = p.DsCodigoReferencia,
                    DsNome = p.DsNome,
                    DtCriacao = p.DtCriacao,
                    DtInicioEstimado = p.DtInicioEstimado,
                    DtTerminoEstimado = p.DtTerminoEstimado,
                    FlStatus = p.FlStatus,
                    IdEmpresa = p.IdEmpresa,
                    IdProjeto = p.IdProjeto,
                    IdTipoProjeto = p.IdTipoProjeto,
                    IdUsuarioCriacao = p.IdUsuarioCriacao
                }).ToList();
        }
    }

    [WebMethod(true)]
    [System.Web.Script.Services.ScriptMethod(UseHttpGet = false, ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public Katapoka.DAO.JsonResponse GetTags(string tag)
    {
        Katapoka.DAO.JsonResponse response = new JsonResponse();
        try
        {
            using (Katapoka.BLL.Tag.TagBLL tagBLL = new Katapoka.BLL.Tag.TagBLL())
            {
                response.Data = tagBLL.GetTagsByName(tag)
                    .Select(p => new Katapoka.DAO.Tag.TagCompleta()
                    {
                        DsTag = p.DsTag,
                        IdTag = p.IdTag
                    }).ToList();
                response.Status = 200;
            }
        }
        catch (Exception ex)
        {
            response.Status = 500;
            response.Data = ex.Message;
        }
        return response;
    }

    /// <summary>
    /// Implementa o ajax reverso de long polling
    /// </summary>
    /// <param name="hashLogin"></param>
    /// <returns></returns>
    [WebMethod]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public string WaitMessage(string hashLogin)
    {
        return (string)Katapoka.AjaxReverso.ClientAdapter.Instance.GetMessage(hashLogin);
    }
}
