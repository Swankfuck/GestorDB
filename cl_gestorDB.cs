using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data; //para poder criar a classe "Public DataTable"
using System.Data.SqlServerCe;//database4
using System.IO;//para saber se existe um ficheiro ou nao (criar se nao existir)
using System.Threading.Tasks;
using System.Windows.Forms;//apresenta uma caixa de texto quando o codigo da erro
// primeiro de tudo coloque os using ! 


//=================Aqui é o que a gente vai criar nesas classe=======================
//classe que permite gerenciar a base de dados
//versao 1.0.0 (29/11/2020)
//===================================================================================

namespace GestorDB
{
    //====================STRING PARA FACILITAR CRIÇÂO DE OBJETO=====================
    public class cl_gestorDB
    {
        SqlCeConnection conectar; // 1º liga a base de dados
        SqlCeCommand comando; // 2/º
        SqlCeDataAdapter adptador;// 3º preenche a base de dados
        //lembrando isso acima foi criado para facilitar na hora de criar objeto
        //exemplo eu criaria o objeto assim
        //SqlCeDataAdapter adptador = new SqlCeDataAdapter();
        //com a string criada a cima eu faço o objeto dessa maneira
        //adptador = new SqlCeDataAdapter();

        //temporario para o 37º 38º
        SqlCeDataAdapter adaptador_temp;



        //=================PARAMETROS PARA CRIAR LISTA CREATE TABLE===================
        //cria a classe de parametros SQL //32º
        public class SQLparametro 
        {
            public string parametro { get; set; }
            public object valor { get; set; } // object é o tipo generico, pode ser string, imagem, inteiro, booleana etc..
            public SQLparametro(string parametro, object valor)
            {
                this.parametro = parametro;// é o valor das duas propriedades acima public string parametro { get; set; }
                this.valor = valor;// é o valor das duas propriedades acima public object valor { get; set; }
            }
        }



        //=======================STRINGS PARA FACILICAR===============================
        string stringconexao = ""; // conectar a data base 4º
        string pasta_bd = @"C:\source\repos\"; //definir a pasta que esta a database exemplo "C:\source\repos" 9º
        string bd_password = "1234"; // definir uma senha para data base 5º

        //===================CLASSE CRIADA PARA NÂO DAR ERRO==========================
        public cl_gestorDB() //26º
        {
            //construtor exclusivo para fazer a seguinte ação
            //cl_gestorDB gestor = new cl_gestorDB(); -> ele serve exclusivamente para criar esse objeto para que não acha conflito com o resto do código
            //gestor.CriarBaseDados(@"C:\source\repos\testandoo.sdf", true);
            //obs: deixar ele vazio !
            //ele foi criado para poder criar um objeto para que o metodo "CriarBaseDados" não de erro.
            //se nao teriamos que criar um objeto a partir do metodo "CriarBaseDados" e daria varios erros pois ele tem bastante funções
        }



        //=====================METODO CRIAR A BASE DE DADOS===========================                  
        public void CriarBaseDados(string base_dados, List<string> instrucoes,bool verificar_ficheiro = false)//criado exclusivamente para criar uma base de dados nova 20º
        {
            //============================================================================

            #region Verificação da existencia de ficheiro da base de dados 21º
            if (verificar_ficheiro)//21º
            {
                //executa a verificação  se for true
                //se for false vai pular esse código
                //base_dados = tem o nome com o caminho completo até a database !!
                if(File.Exists(base_dados))
                {
                    if (MessageBox.Show("Existe uma base de dados com o mesmo nome." + //texto que vai aparecer na mensagem
                                        Environment.NewLine + // paragrafo
                                        "Deseja subistuir a base de dados existente ?", //texto que vai aparecer na mensagem
                                        "ATENÇÃO", //titulo da box
                                        MessageBoxButtons.YesNo, //botao de sim ou nao
                                        MessageBoxIcon.Warning) == DialogResult.No) //icone de cuidado e se o resultado for NAO
                    { 
                        return; // nao retorna qualquer valor e permanece na apicação
                    }
                    else
                    {
                        //se o usuario apertar Sim, apaga o ficheiro existente e cria um novo  29º
                        try
                        {
                            File.Delete(base_dados);
                        }
                        catch
                        {
                            MessageBox.Show("Erro, é necessario fechar os processos que estão abertos."); //se o programa tive aberto não tem como deletar o arquivo
                            return;
                        }
                    }
                }
                   
            }
            #endregion

            //Construção da stringconexao 23º
            #region Construção da stringconexao (Data Source) 23º
            StringBuilder str = new StringBuilder();
            str.Append("Data source = ");
            #endregion


            //nome da base de dados 24º
            #region nome da base de dados (base_dados)25º
            str.Append(base_dados);
            #endregion

            //verifica se tem password 25º
            #region verifica se tem password (bd_password) 26º
            if (bd_password != "")
                str.Append("; Password = " + bd_password);
            #endregion


            //criação da base de dados 22º
            #region Criação da base de dados (CreateDataBase MOTOR) 22º
            SqlCeEngine motor = new SqlCeEngine(str.ToString()); //tudo que ta aqui dentro () eu criei aqui em cima /\
            motor.CreateDatabase();
            #endregion

            //veriifcar se tudo esta funcionado com esse codigo do botao 1   28º
            #region para veriifcar se tudo esta funcionado com esse codigo do botao 1(para teste apenas)
            //cl_gestorDB gestor = new cl_gestorDB(); //repare que essa classe cl_gestorDB é a que não possui nada
            //gestor.CriarBaseDados(@"C:\source\repos\loja.sdf", true);
            //MessageBox.Show("Base de dados criada");
            #endregion

            //============================================================================
            
            //criação das tabelas dentro da base de dados 30º
            #region criação das tabelas dentro da base de dados 30º
            //======================EXEMPLO================================================
            //1º CREATE TABLE e o nome da tabela
            //2º id_contato         int not null primary key
            //3º nome               nvarchar(50)
            //4º telefone           nvarchar(50)
            //5º data               datetime
            //6º etc...

            //END -> ao colocar END ele faz o seguinte quando ele termina de ler a tabela acima /\
            //ao chegar no END ele finaliza a tabela e vai para a proxima abaixo \/
            //então da para criar duas tabelas ou mais seguidas
            //exemplo tabelas de clientes
            //end
            //tabelas de encomendas ou compras
            //end
            //etc...

            //1º CREATE TABLE e o nome da tabela
            //2º id_contato         int not null primary key
            //3º nome               nvarchar(50)
            //4º telefone           nvarchar(50)
            //5º data               datetime
            //6º etc...
            //============================================================================


            conectar = new SqlCeConnection(str.ToString());//comando para ligar a base de dados
            conectar.Open();
            comando = new SqlCeCommand();//comando para injetar instruções a base de dados
            comando.Connection = conectar;

            //instruções (repare que eu coloquei aqui list<string> instrucoes "public void CriarBaseDados(string base_dados, "List<string> instrucoes" ,bool verificar_ficheiro = false)
            //instrucoes que eu pretendo ser executadas para criar a tabela
            str = null; // null porque criar sempre um novo conjunto de instruções 
            foreach (string item in instrucoes)
            {
                if(item.StartsWith("CREATE TABLE"))//startWith = começãr por "create table"
                {
                    //inicia a construção da querry
                    str = new StringBuilder();// se começa com CREATE TABLE ele coloca o "item"
                    str.Append(item);
                }
                else if(item == "END") //END para terminar a tabela acima e executar o código abaixo \/
                {
                    //fechar a criação da query e executa-la
                    comando.CommandText = str.ToString();
                    comando.ExecuteNonQuery();//aperte f9 aqui para testar se a base de dados ta correta

                }
                else//adiciona tudo que esta após ao "CREATE TABLE" ou seja as instruçõse "id_Contato, nome etc.. até chegar no END. 
                {
                    //adicionar instrução ao stringbuilder
                    str.Append(item);
                }

            }
            




            //fecha o comando e a ligação 
            comando.Dispose();
            conectar.Dispose();






            #endregion


        }
        


        //========================ABRIR A BASE DE DADOS===============================
        public cl_gestorDB(string base_dados)//6º definir o nome da base de dados(caminho,nome)
        {
            //definir a connectionString da conexão 8º
            //Maneira normal de se criar \/
            //stringconexao = "Data source = " + pasta_bd + base_dados + ".sdf; Password = " + bd_password;

            //Maneira diferente de criar com o stringbuilder é mais eficiente na concatenação !!\/ 10º
            StringBuilder str = new StringBuilder();


            //define a base da stringconexao 11º
            str.Append("Data source = ");

            //verifica se existe pasta definida (localização)
            if (pasta_bd != "")//12º
            {
                str.Append(pasta_bd); //caso existir ele pula pro código abaixo e não executa esse

            }

            //acrescenta o nome do ficheiro da base de dados //13º
            str.Append(base_dados + ".sdf");//nao esqueça de adicionar o .sdf para facilitar 
            //se nao existir password ela termina aqui /\

            //adiciona password (se tiver o codigo continua aqui) 14º
            if (bd_password != "")
            {
                str.Append("; Password = " + bd_password);
            }

            //define o stringconexao 15º
            stringconexao = str.ToString();
            //MessageBox.Show(stringconexao); para ver se deu certo 17º

            //============================================================================
            //temporario para ver se conectou com a stringconexao 18º
            //conectar = new SqlCeConnection(stringconexao);
            //conectar.Open();
            //MessageBox.Show(conectar.State.ToString());
            //conectar.Dispose();
        }



        //==================BUSCAR INFORMAÇÔES NA BASE DE DADOS=======================
        public DataTable EXE_READER(string query, List<SQLparametro> parametros = null)
        {
            //efetuar pesquisa no textbox 31º
            //só o SELECT vai ser usado 

            DataTable dados = new DataTable();
            adptador = new SqlCeDataAdapter(query, stringconexao);
            adptador.SelectCommand.Parameters.Clear();//limpa os parametros que existe dentro do adptador

            //executar a querry
            try
            {
                //insere os parametros na query(caso exista para adicionar)
                if(parametros != null)
                {
                    foreach (SQLparametro Par in parametros)
                        adptador.SelectCommand.Parameters.AddWithValue(Par.parametro, Par.valor);
                        
                    
                }

                adptador.Fill(dados);
            }
            catch (Exception ex)//Exception apresenta um erro que ocorre durante a aplicação 
            //ex é a variavel aonde vai ser mostrado
            {
                MessageBox.Show("ERRO: " + ex.Message);//para mostrar o erro encontrado pelo Exception
                //lembrando não coloque isso no código oficial pois o erro que aparece é apenas para quem esta programando ver e corrigir !! 
            }
            adptador.Dispose();
            return dados;
        }



        //==================GRAVAR INFORMAÇÔES NA BASE DE DADOS========================
        public void EXE_NON_QUERY(string query, List<SQLparametro> parametros = null)
        {
            //33º
            //VOID = não devolve dados, apenas insere dados !
            //parametros NULL = NULL para poder usar com duas assinaturas destinta, 1 só com a "query" e outra só com "List<SQLparametros> parametros"

            //executar queries do tipo INSERT , DELETE , UPDATE , CREATE TABLE, etc...
            conectar = new SqlCeConnection(stringconexao);
            conectar.Open();

            comando = new SqlCeCommand(query, conectar);
            comando.Parameters.Clear();

            try
            {
                //adição de parametros no comando
                if(parametros != null)
                {
                    foreach (SQLparametro P in parametros)
                    {
                        comando.Parameters.AddWithValue(P.parametro, P.valor); //parametro e valor esta aqui " public class SQLparametro "
                    }
                }
                //executa a query
                comando.ExecuteNonQuery();
            }
            catch(Exception ex)//para o sistema me falar o erro (nao por para o usuario ver!)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }

            //desligar o conectar e o comando
            comando.Dispose();
            conectar.Dispose();

        }



        //======METODO PARA BUSCAR O PROXIMO ID DISPONIVEL PARA GRAVAR INFORMAÇÂO======
        public int ID_DISPONIVEL(string tabela, string coluna)
        {
            //36º
            //devolve o ID disponivel para o proximo registro

            int id_temp = -1;

            string query = "SELECT MAX(" + coluna + ") AS maxid FROM " + tabela;
            DataTable dados = new DataTable();
            adptador = new SqlCeDataAdapter(query, stringconexao);
            adptador.Fill(dados);
            
            //é preciso fazer isso porque se nao tiver id_cliente nenhum cadastrado vai dar erro por causa do "SELECT MAX"
            //verifica se é DBNull ou não (se não id_cliente cadastrador ou não)
            if(dados.Rows.Count !=0)
            {
                if (DBNull.Value.Equals(dados.Rows[0][0]))
                    id_temp = 0;
                else
                    id_temp = Convert.ToInt16(dados.Rows[0][0]) + 1; //para que adicione o contato no proximo id_contato
                                                                     //exemplo o ultimo id_contato é o 2 ele vai fazer +1 e salver o id_contato novo no id_contato 3
            }
                                              
            return id_temp;
        }



        //===================EXEMPLO PARA GUARDAR VARIOS DADOS=========================
        //usado pelo programador
        //37º
        //1º metodo para preparar uma DataTable
        public DataTable PREPARAR_DATATABLE(string query)
        {
            //preparar a datatable para atualização ou inserção de dados
            adaptador_temp = new SqlCeDataAdapter(query, stringconexao);
            DataTable dados = new DataTable();

            try
            {
                adaptador_temp.Fill(dados);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERRO: " + ex.Message);     
            }

            return dados;
        }



        //===================EXEMPLO PARA SALVAR VARIOS DADOS==========================
        //usado pelo programador
        //38º
        //2º metodo para atualizar a base de dados com os dados novos  
        public void GRAVAR_DATATABLE(DataTable dados)
        {
            //atualiza a informação na base de dados
            SqlCeCommandBuilder CMD = new SqlCeCommandBuilder(adaptador_temp);
            adaptador_temp.UpdateCommand = CMD.GetUpdateCommand();

            try
            {
                adaptador_temp.Update(dados);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERRO: " + ex.Message);         
            }

            adaptador_temp.Dispose();

        }



        //=================METODO PARA COMPACTAR A BASE DE DADOS=======================
        //40º
        //compactar a base de dados = ao usuario excluir um registro fica uma linha em branco
        //aqui veremos um metodo para apagar essas linhas em branco e compactar a base de dados
        public bool COMPACTAR_BASEDADOS()
        {
            bool concluido = false;
            

            try
            {
                SqlCeEngine motor = new SqlCeEngine();
                motor.LocalConnectionString = stringconexao;
                motor.Compact(stringconexao);
                concluido = true;
            }
            catch (Exception ex)
            {
                concluido = false;
                MessageBox.Show("ERROR: " + ex.Message);//so coloco esse erro para min para o usario nao posso por Exception
            }

            return concluido;
            //o usuario não precisa saber da existencia desse metodo
            //então nao coloque botoes de compactar base de dados, apenas adicione no codigo
            //lembrando que a base de dados precisa estar fechada no pc principal do codigo !!!!

            //executar ao atingir 100 registros, de 100 em 100 seria legal.

        }

    }
} 
