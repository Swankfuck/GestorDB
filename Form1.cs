using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestorDB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //============================================================================
        private void button1_Click(object sender, EventArgs e)
        {
            //=========================EXEMPLOS===========================================
            //instancia da classe gestora
            //cl_gestorDB gestor = new cl_gestorDB("nome da base de dados"); //7º

            //cl_gestorDB gestor = new cl_gestorDB("loja"); //repare que nao precisei por .sdf pois coloquei la atras no str 16º
            //definindo a pasta_bd com o caminho "C:\source\repos" eu só preciso por o nome do arquivo da database ali.

            //para abrir uma outra database junto com a de cima faça o seguinte \/ 19º
            // gestor = new cl_gestorDB("nome da database");


            //============================================================================ 27º
            cl_gestorDB gestor = new cl_gestorDB(); //repare que essa classe cl_gestorDB é a que não possui nada

            //define as instrucoes das tabelas 30º
            List<string> instrucoes = new List<string>()
            {
                //tabela clientes
                "CREATE TABLE clientes (",
                "id_cliente             int not null primary key,",
                "nome                   nvarchar(50),",
                "telefone               nvarchar(20),",
                "data                   datetime)",
                "END", //a 1º tabela termina aqui

                //tabela compras
                "CREATE TABLE compras (",
                "id_compra              int not null primary key,",
                "id_cliente             int, ",
                "produto                nvarchar(100),",
                "quantidade             int,",
                "preco_unidade          int,",
                "data_compra            datetime,",
                "FOREIGN KEY(id_cliente) REFERENCES clientes(id_cliente) ON DELETE CASCADE)", //sempre que deleto um cliente deleta todo historico de compra dele (por isso cascade)
                "END" //a 2º tabela termina aqui

            };
            




            gestor.CriarBaseDados(@"C:\source\repos\mercado.sdf", instrucoes ,true);//e esse gestor é da cl_gestorDB que possui o a string "base_dados"
            //isso foi feito para nao criar conflito com o a string "base_dados"
        }
        //============================================================================
        private void button2_Click(object sender, EventArgs e)
        {
            //abertura da base de dados
            cl_gestorDB gestor = new cl_gestorDB("mercado"); //coloquei sómente o nome da database que é mercado.sdf 
            
            
            //============================================================================
            //construção da lista de parametros 32º
            List<cl_gestorDB.SQLparametro> parametros = new List<cl_gestorDB.SQLparametro>();
            parametros.Add(new cl_gestorDB.SQLparametro("@item_pesquisa", textBox1.Text));
           
            //executa a "query na data base"
            DataTable dados = gestor.EXE_READER("SELECT * FROM clientes WHERE nome = @item_pesquisa", parametros);

            //============================================================================
            //somente para texte para ver as linhas devolvidas
            MessageBox.Show(dados.Rows.Count.ToString());  
        }
        //============================================================================
        private void button3_Click(object sender, EventArgs e)
        {
            //abertura da base de dados
            cl_gestorDB gestor = new cl_gestorDB("mercado"); //coloquei sómente o nome da database que é mercado.sdf 

            //repare que nao tem a lista de parametro na concatenação
            //repare que se o usuario colocar o nome assim 'vitor' ele vai da erro na sql(o famoso sqlInjetc)
            //ESSE JEITO È ERRADO DE FAZER
            //O JEITO CORRETO È O DE CIMA COM PARAMETROS !!

            //executa a "query na data base"
            DataTable dados = gestor.EXE_READER("SELECT * FROM clientes WHERE nome = '" + textBox1.Text + "'");

            //============================================================================
            //somente para texte para ver as linhas devolvidas
            MessageBox.Show(dados.Rows.Count.ToString());  
        }
        //============================================================================
        private void button4_Click(object sender, EventArgs e) //34º
        {

            //inserir um novo contato direto do código abaixo \/
            cl_gestorDB gestor = new cl_gestorDB("mercado"); //conexao a base de dados

            //criar lista de parametros
            List<cl_gestorDB.SQLparametro> parametros = new List<cl_gestorDB.SQLparametro>();
            parametros.Add(new cl_gestorDB.SQLparametro("@id_cliente", 1));
            parametros.Add(new cl_gestorDB.SQLparametro("@nome", "rosana"));
            parametros.Add(new cl_gestorDB.SQLparametro("@telefone", "1245467"));
            parametros.Add(new cl_gestorDB.SQLparametro("@atualizacao", DateTime.Now));

            gestor.EXE_NON_QUERY("INSERT INTO clientes VALUES(" +
                                "@id_cliente," +
                                " @nome," +
                                " @telefone," +
                                " @atualizacao)", parametros);

            MessageBox.Show("deu certo");


            //============================================================================
            //REPARE È ASSIM QUE SERIA SE NAO TIVESSE O PARAMETRO ACIMA
            //gestor.EXE_NON_QUERY("INSERT INTO clienst VALUES(" +
            //                    1 + "," +
            //                    "'rosana'," +
            //                    "'124567'," +
            //                    "'" + DateTime.Now + "'"); //e ainda daria erro aqui no datetime.
            //============================================================================

        }
        //============================================================================
        private void button5_Click(object sender, EventArgs e)
        {
            //35º (inserir nome e telefone no textbox)

            //verifica se os campos estão preenchidos
            if(text_nome.Text == "" || text_telefone.Text == "")
            {
                MessageBox.Show("É preciso preencher os campos obrigatórios !", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            //grava um novo contato na base de dados
            cl_gestorDB gestor = new cl_gestorDB("mercado");

            int id_cliente = gestor.ID_DISPONIVEL("clientes", "id_cliente"); //vai buscar o proximo id_cliente disponivel para adicionar o contato (feito no 36º)
            //ou seja se tiver id_cliente até o 2, ele vai procurar o 3 para salvar o novo contato


            //parametros 
            List<cl_gestorDB.SQLparametro> paramentros = new List<cl_gestorDB.SQLparametro>();
            paramentros.Add(new cl_gestorDB.SQLparametro("@id_cliente", id_cliente));
            paramentros.Add(new cl_gestorDB.SQLparametro("@nome", text_nome.Text));
            paramentros.Add(new cl_gestorDB.SQLparametro("@telefone", text_telefone.Text));
            paramentros.Add(new cl_gestorDB.SQLparametro("@atualizacao", DateTime.Now));
            
            
            gestor.EXE_NON_QUERY("INSERT INTO clientes VALUES(" +
                                            "@id_cliente," +
                                            " @nome," +
                                            " @telefone," +
                                            " @atualizacao)", paramentros);

            MessageBox.Show("Contato adicionado.");
        }
        //============================================================================
        private void button6_Click(object sender, EventArgs e)
        {
            //prepara uma DataTable 39º (vai usar o 37 e 38)

            cl_gestorDB gestor = new cl_gestorDB("mercado");

            DataTable dados = gestor.PREPARAR_DATATABLE("SELECT * FROM clientes");

            //alterações que forem necessarias
            foreach (DataRow linha in dados.Rows)
            {
                //alterar o telefone atraves da linha de código (não é o usuario que altera aqui!)
                //vai fazer todos os telefones ficar com 017 na frente (poderia por outro ddd tambem)
                //codigo usado somente pelo progrador!
                string novo_telefone = "(17) " + linha["telefone"].ToString();
                linha["telefone"] = novo_telefone;
            }

            gestor.GRAVAR_DATATABLE(dados);
        }
        //============================================================================
        private void button7_Click(object sender, EventArgs e)
        {
            //39º
            //inserir novos clientes na base de dados
            //código usado pelo programador para inserir varios dados de uma só vez

            List<string> novos_nomes = new List<string>();
            novos_nomes.Add("nome 1");
            novos_nomes.Add("nome 2");
            novos_nomes.Add("nome 3");
            novos_nomes.Add("nome 4");
            novos_nomes.Add("nome 5");

            List<string> novos_telefones = new List<string>();
            novos_telefones.Add("1");
            novos_telefones.Add("12");
            novos_telefones.Add("123");
            novos_telefones.Add("1234");
            novos_telefones.Add("12345");

            cl_gestorDB gestor = new cl_gestorDB("mercado");
            DataTable dados = gestor.PREPARAR_DATATABLE("SELECT * FROM clientes WHERE id_cliente = -1");

            int id_temp = gestor.ID_DISPONIVEL("clientes", "id_cliente");
            //quando o codigo chegar aqui /\ ele vai ver qual o proximo id_cliente disponivel
            //e vai pular para o código abaixo \/ para ser inserido os clientes da linha acima



            //percorrer todos os nomes e adicionar a base de dados
            //enquanto o indice(index) for menor que o numero de elelementos da lista novos_nomes
            for (int index = 0; index < novos_nomes.Count ; index++)
            {
                DataRow nova_linha = dados.NewRow();
                nova_linha["id_cliente"] = id_temp;
                nova_linha["nome"] = novos_nomes[index];
                nova_linha["telefone"] = novos_telefones[index];

                dados.Rows.Add(nova_linha);
                id_temp++;//para que passe para o proximo id_cliente
            }

            gestor.GRAVAR_DATATABLE(dados); // para salvar os dados acima /\
        }
        //============================================================================
        private void button8_Click(object sender, EventArgs e)
        {
            //41º (usado o 40º)
            cl_gestorDB gestor = new cl_gestorDB("mercado"); 
            if(gestor.COMPACTAR_BASEDADOS() == true)
            {
                MessageBox.Show("deu certo");
            }
            
        }
    }
}
