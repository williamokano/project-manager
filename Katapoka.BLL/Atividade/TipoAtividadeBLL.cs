using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EntityClient;
using System.Linq;
using System.Text;
using Katapoka.DAO;

namespace Katapoka.BLL.Atividade
{
    public class TipoAtividadeBLL : AbstractBLLModel<Katapoka.DAO.TipoAtividade_Tb>
    {
        public IList<Katapoka.DAO.Atividade.TipoAtividade> GetTipoAtividadePorTipoProjeto(int idTipoProjeto)
        {
            string query = @"SELECT
	                            ta.IdTipoAtividade,
	                            ta.DsTituloAtividade,
	                            ta.DsAtividade,
	                            ta.QtTempoEstimado,
	                            tapre.IdTipoAtividadePredecessora,
	                            tpta.IdTipoProjeto
                            FROM
	                            TipoAtividade_Tb ta
                            INNER JOIN
	                            TipoProjetoTipoAtividade_Tb tpta	
	                            ON ta.IdTipoAtividade = tpta.IdTipoAtividade
	                            AND tpta.IdTipoProjeto = @IdTipoProjeto
                            LEFT JOIN
	                            TipoProjetoTipoAtividadePredecessora_Tb tapre
	                            ON tapre.IdTipoAtividade = ta.IdTipoAtividade AND
	                            tapre.IdTipoProjeto = @IdTipoProjeto;";

            IDbConnection cnn = (Context.Connection as EntityConnection).StoreConnection;

            try
            {
                cnn.Open();
                IDbCommand cmd = cnn.CreateCommand();
                cmd.CommandText = query;

                System.Data.SqlClient.SqlParameter paramIdTipoProjeto = new System.Data.SqlClient.SqlParameter("@IdTipoProjeto", SqlDbType.Int);
                paramIdTipoProjeto.Direction = ParameterDirection.Input;
                paramIdTipoProjeto.Value = idTipoProjeto;
                cmd.Parameters.Add(paramIdTipoProjeto);

                DbDataReader dbReader = (DbDataReader)cmd.ExecuteReader();
                IList<Katapoka.DAO.Atividade.TipoAtividade> tiposAtividade = Context.Translate<Katapoka.DAO.Atividade.TipoAtividade>(dbReader).ToList();

                return tiposAtividade;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }

            //var query = this.Context.TipoAtividade_Tb
            //    .Join(this.Context.TipoProjetoTipoAtividade_Tb, p => p.IdTipoAtividade, p => p.IdTipoAtividade, (tb1, tb2) => new { TipoAtividade_Tb = tb1, TipoProjetoTipoAtividade_Tb = tb2 })
            //    .Join(this.Context.TipoProjeto_Tb, p => p.TipoProjetoTipoAtividade_Tb.IdTipoProjeto, p => p.IdTipoProjeto, (tb1, tb2) => new { TipoAtividade_Tb = tb1.TipoAtividade_Tb, TipoProjetoTipoAtividade_Tb = tb1.TipoProjetoTipoAtividade_Tb, TipoProjeto_Tb = tb2 })
            //    .Join(this.Context.TipoProjetoTipoAtividadePredecessora_Tb, p => p.TipoAtividade_Tb.IdTipoAtividade
            //
            //query = query.Where(p => p.TipoProjeto_Tb.IdTipoProjeto == idTipoProjeto);
            //return query.Select(p => p.TipoAtividade_Tb)
            //    .ToList();
        }
        public TipoProjetoTipoAtividadePredecessora_Tb GetTipoProjetoTipoAtividadePredecessora(int idTipoProjeto, int idTipoAtividade)
        {
            return this.Context.TipoProjetoTipoAtividadePredecessora_Tb
                .Where(p => p.IdTipoProjeto == idTipoProjeto && p.IdTipoAtividade == idTipoAtividade)
                .FirstOrDefault();
        }
    }
}
