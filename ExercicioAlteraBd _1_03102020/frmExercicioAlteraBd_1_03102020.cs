//EXERCICIO ALTERAR BD COM BNDNAV
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//Utilizando as bibliotecas de classe do DOTNET.
//IO - Contém dezenas de classes. A classe "StreamReader" implementa um TextReader (Leitor de texto) 
using System.IO;

//ADO.NET é um conjunto de assemblies (Classes) que faz parte do .NET e que permite a 
//comunicação com os bancos de dados realizando operações de leitura e atualizações.
using System.Data.OleDb;

namespace ExercicioAlteraBd__1_03102020
{
    public partial class frmExercicioAlteraBd_1_03102020 : Form
    {
        StreamReader objLeitor;
        string strLinhaLida, strValorAntigo, strValorNovo;

        OleDbConnection objConexao;
        OleDbCommand objComando;
        OleDbDataReader objLeitorBd;
        OleDbDataAdapter objAdaptador;

        DataTable objTabela;

        bool bolPreferenciaInserida;

        public frmExercicioAlteraBd_1_03102020()
        {
            InitializeComponent();
        }

        private void dtgdvwPreferencias_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            strValorAntigo = dtgdvwPreferencias.CurrentCell.Value.ToString();
            if (!string.IsNullOrEmpty(dtgdvwPreferencias.CurrentCell.EditedFormattedValue.ToString()))
            {
                strValorNovo = dtgdvwPreferencias.CurrentCell.EditedFormattedValue.ToString();
            }
        }

        private void btnDesvCond_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Clique em Ok ou Cancelar", "Desvio Condicional", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show("Você Clicou em ok");
            }
            else
            {
                MessageBox.Show("Você Clicou em Cancelar");
            }
        }

        private void btnImportaTextoWhile_Click(object sender, EventArgs e)
        {
            lstbxPreferencias.Items.Clear();
            ImportarTextoWhile();
        }
        public void ImportarTextoWhile()
        {
            objLeitor = new StreamReader(@"D:\Curso Programa\Preferencias.txt");
            strLinhaLida = objLeitor.ReadLine();
            while(strLinhaLida != null)
            {
                lstbxPreferencias.Items.Add(strLinhaLida);
                strLinhaLida = objLeitor.ReadLine();
            }

            objLeitor.Close();

        }

        private void btnImportaBdCon_Click(object sender, EventArgs e)
        {
            lstbxPreferencias.Items.Clear();
            ImportarBdCon();
        }
        public void ImportarBdCon()
        {
            objConexao = new OleDbConnection();
            objConexao.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='D:\Curso Programa\preferencias1.mdb'";
            objConexao.Open();

            objComando = new OleDbCommand();
            objComando.Connection = objConexao;
            objComando.CommandText = "SELECT DEscricao FROM Preferências_3";

            objLeitorBd = objComando.ExecuteReader();

            while (objLeitorBd.Read())
            {
                lstbxPreferencias.Items.Add(objLeitorBd["Descricao"].ToString());
            }
            objConexao.Close();
        }

        private void btnImportaBdDesc_Click(object sender, EventArgs e)
        {
            lstbxPreferencias.Items.Clear();
            ImportarBdDesc();
        }
        public void ImportarBdDesc()
        {
            objConexao = new OleDbConnection();
            objConexao.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='D:\Curso Programa\preferencias1.mdb'";

            objComando = new OleDbCommand();
            objComando.Connection = objConexao;
            objComando.CommandText = "SELECT DEscricao FROM Preferências_3";

            objAdaptador = new OleDbDataAdapter();
            objAdaptador.SelectCommand = objComando;

            objTabela = new DataTable();
            objAdaptador.Fill(objTabela);

            foreach(DataRow drItemDaTabela in objTabela.Rows)
            {
                lstbxPreferencias.Items.Add(drItemDaTabela["Descricao"].ToString());
            }
        }

        private void btnConsultaBd_Click(object sender, EventArgs e)
        {
            ConsultarBd();
        }
        public void ConsultarBd(string strparDescricaoDaPreferencia = null)
        {
            objConexao = new OleDbConnection();
            objConexao.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='D:\Curso Programa\preferencias1.mdb'";

            objComando = new OleDbCommand();
            objComando.Connection = objConexao;

            if (string.IsNullOrEmpty(strparDescricaoDaPreferencia))
            {
                objComando.CommandText = "SELECT DEscricao FROM Preferências_3";
            }
            else
            {
                objComando.CommandText = "SELECT DEscricao FROM Preferências_3 WHERE DEscricao = '" + strparDescricaoDaPreferencia + "'";

            }

            objAdaptador = new OleDbDataAdapter();
            objAdaptador.SelectCommand = objComando;

            objTabela = new DataTable();
            objAdaptador.Fill(objTabela);

            bndsrcPreferencias.DataSource =  objTabela;                       
            dtgdvwPreferencias.DataSource = bndsrcPreferencias;
        }

        private void frmExercicioAlteraBd_1_03102020_Load(object sender, EventArgs e)
        {
            ConsultarBd();
        }

        private void btnIncluiBd_Click(object sender, EventArgs e)
        {
            IncluirBd(dtgdvwPreferencias.CurrentCell.Value.ToString());

            ConsultarBd();
        }
        public void IncluirBd(string strPreferenciaIncluida)
        {
            objConexao = new OleDbConnection();
            objConexao.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='D:\Curso Programa\preferencias1.mdb'";
            objConexao.Open();

            objComando = new OleDbCommand();
            objComando.Connection = objConexao;
            objComando.CommandText = "INSERT INTO Preferências_3 (Descricao) VALUES ('" + strPreferenciaIncluida + "')";

            if (objComando.ExecuteNonQuery() > 0)
            {
                MessageBox.Show("Registro Incluído");
            }
            else
            {
                MessageBox.Show("Problemas na inclusão do Registro");
            }
            objConexao.Close();
        }

        private void btnEcluiBd_Click(object sender, EventArgs e)
        {
            ExcluirBd(dtgdvwPreferencias.CurrentCell.Value.ToString());

            ConsultarBd();
        }
        public void ExcluirBd(string strPreferenciaExcluida)
        {
            objConexao = new OleDbConnection();
            objConexao.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='D:\Curso Programa\preferencias1.mdb'";
            objConexao.Open();


            objComando = new OleDbCommand();
            objComando.Connection = objConexao;
            objComando.CommandText = "DELETE FROM Preferências_3 WHERE Descricao = '" + strPreferenciaExcluida + "'";

            if (objComando.ExecuteNonQuery() > 0)
            {
                MessageBox.Show("Registro Excluído");
            }
            else
            {
                MessageBox.Show("Problemas na Exclusão do Registro");
            }
            objConexao.Close();

        }

        private void btnAlteraBd_Click(object sender, EventArgs e)
        {
            AlterarBd(strValorAntigo, dtgdvwPreferencias.CurrentCell.EditedFormattedValue.ToString());

            ConsultarBd();
        }
        public void AlterarBd(string strPreferenciaAntiga, string strPreferenciaNova)
        {
            objConexao = new OleDbConnection();
            objConexao.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='D:\Curso Programa\preferencias1.mdb'";
            objConexao.Open();


            objComando = new OleDbCommand();
            objComando.Connection = objConexao;
            objComando.CommandText = "UPDATE Preferências_3 SET Descricao = '" + dtgdvwPreferencias.CurrentCell.EditedFormattedValue.ToString() + "' WHERE Descricao = '" + strValorAntigo + "'";

            if (objComando.ExecuteNonQuery() > 0)
            {
                MessageBox.Show("Registro Alterado");
            }
            else
            {
                MessageBox.Show("Problemas na Alteração do Registro");
            }
            objConexao.Close();
        }

        private void bndNavBtnPesquisa_Click(object sender, EventArgs e)
        {
            ConsultarBd(bndNavTxtPesquisa.Text);
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            bolPreferenciaInserida = true;
        }

        private void bndNavBtnConfirmar_Click(object sender, EventArgs e)
        {
            if (bolPreferenciaInserida)
            {
                if (MessageBox.Show("Confirma a inclusão da Preferência " + dtgdvwPreferencias.CurrentCell.EditedFormattedValue.ToString() + "?", "Inclusão no Banco de Dados", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    IncluirBd(dtgdvwPreferencias.CurrentCell.EditedFormattedValue.ToString());
                }
                bolPreferenciaInserida = false;
            }
            else
            {
                if (MessageBox.Show("Confirma a alteração da Preferência " + strValorAntigo + " pela preferência nova " + dtgdvwPreferencias.CurrentCell.EditedFormattedValue.ToString() + "?", "Alteração no Banco de Dados", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    AlterarBd(strValorAntigo, strValorNovo);
                }
            }
            ConsultarBd();
        }

        private void bndnavbtnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirma a exclusão da Preferência " + strValorAntigo + "?", "Exclusão no Banco de Dados", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                ExcluirBd(strValorAntigo);
            }
            ConsultarBd();
        }
    }
}