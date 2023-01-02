using System;
using System.Text.Json.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;


namespace JogoDaVelha
{
    class Program
    {

        static void MostrarJogoDaVelha(string jogador, string[,] tabuleiro)
        {

            Console.Clear();
            Console.WriteLine("Jogo da velha\n");
            Console.WriteLine($" {tabuleiro[0,0]} | {tabuleiro[0,1]} | {tabuleiro[0,2]} ");
            Console.WriteLine($"---|---|---");
            Console.WriteLine($" {tabuleiro[1,0]} | {tabuleiro[1,1]} | {tabuleiro[1,2]} ");
            Console.WriteLine($"---|---|---");
            Console.WriteLine($" {tabuleiro[2,0]} | {tabuleiro[2,1]} | {tabuleiro[2,2]} ");

            string nomeMarcador = "";
            if (jogador == "1") nomeMarcador = "xis";
            else if (jogador == "2") nomeMarcador = "círculo";

            Console.Write($"\nJogador {jogador}, digite um dos número acima para marcar com {nomeMarcador}: ");
            
        }

        static string VerificarGanhador(string[,] tabuleiro)
        {

            string marcador = "";
            bool verificaLinhas = true;
            bool verificaDiagonais = false;
            string tresXis = "XXX";
            string tresCirculos = "OOO";

            while (true)
            {

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (verificaLinhas)
                        {
                            marcador += tabuleiro[i, j];
                        }
                        else
                        {
                            marcador += tabuleiro[j, i];
                            verificaDiagonais = true;
                        }
                    }
                    if (marcador == tresXis) return "1";
                    else if (marcador == tresCirculos) return "2";

                    marcador = "";
                }

                verificaLinhas = false;

                if (verificaDiagonais)
                {
                    string diagonalPrincipal = $"{tabuleiro[0, 0]}{tabuleiro[1, 1]}{tabuleiro[2, 2]}";
                    string diagonalSecundaria = $"{tabuleiro[0, 2]}{tabuleiro[1, 1]}{tabuleiro[2, 0]}";                    

                    if(diagonalPrincipal == tresXis || diagonalSecundaria == tresXis)
                    {
                        return "1";
                    }
                    else if(diagonalPrincipal == tresCirculos || diagonalSecundaria == tresCirculos)
                    {
                        return "2";
                    }

                    return "";
                }

            }

        }

        public class HistoricoPartidas
        {

            public int VitoriasJogador1 { get; set; }
            public int VitoriasJogador2 { get; set; }
            public int Empates { get; set; }
            public int QuantidadePartidas { get; set; }

        }

        static string ConverterParaJSON(int vitoriasJogador1, int vitoriasJogador2, int empates, int quantidadePartidas)
        {

            var opcoes = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var partida = new HistoricoPartidas
            {
                VitoriasJogador1 = vitoriasJogador1,
                VitoriasJogador2 = vitoriasJogador2,
                Empates = empates,
                QuantidadePartidas = quantidadePartidas
            };

            var json = JsonSerializer.Serialize(partida, opcoes);

            return json;

        }

        static void ConverterParaObjeto(string json)
        {

            var partida = JsonSerializer.Deserialize<HistoricoPartidas>(json);

            Console.WriteLine("Histórico das partidas anteriores\n");
            Console.WriteLine($"O Jogador 1 obteve {partida.VitoriasJogador1} vitória(s)");
            Console.WriteLine($"O Jogador 2 obteve {partida.VitoriasJogador2} vitória(s)");
            Console.WriteLine($"Houve {partida.Empates} empate(s)");
            Console.WriteLine($"O total de partidas foi {partida.QuantidadePartidas}\n\n");

        }

        static void SalvarDadosJogo(int vitoriasJogador1, int vitoriasJogador2, int empates, int quantidadePartidas)
        {

            string nomeArquivo = @"historicoPartidasAnteriores.txt";

            try
            {

                using (StreamWriter instanciaEscrita = File.CreateText(nomeArquivo))
                {

                    string json = ConverterParaJSON(vitoriasJogador1, vitoriasJogador2, empates, quantidadePartidas);
                    instanciaEscrita.WriteLine(json);

                }

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }

        }

        static string ObterDadosJogo()
        {

            string nomeArquivo = @"historicoPartidasAnteriores.txt";
            string dadosArquivo = "";

            try
            {
                if (File.Exists(nomeArquivo))
                {
                    using (StreamReader instanciaLeitura = File.OpenText(nomeArquivo))
                    {

                        string linhaArquivo = "";

                        while ((linhaArquivo = instanciaLeitura.ReadLine()) != null)
                        {
                            dadosArquivo += linhaArquivo;
                        }

                        return dadosArquivo;

                    }
                }

                return "";

            }
            catch (Exception Ex)
            {
                return "";
            }

        }

        static void MostrarHistoricoPartidasAnteriores()
        {

            string dadosJogo = ObterDadosJogo();
            if (dadosJogo != "") ConverterParaObjeto(dadosJogo);

            Console.WriteLine("Pressione qualquer tecla para jogar uma nova partida");
            Console.ReadKey();

        }

        static void Jogar()
        {

            string[,] tabuleiro = { { "1", "2", "3" }, { "4", "5", "6" }, { "7", "8", "9" } };
            string jogador = "1";
            string marcador = "";
            bool haVencedor = false;
            string vencedor = "";
            int qtdeMarcacoes = 0;
            bool houveEmpate = false;
            int vitoriasJogador1 = 0;
            int vitoriasJogador2 = 0;
            int empates = 0;
            int quantidadePartidas = 0;
            bool houvePartida = false;
            bool sairPrograma = false;

            MostrarJogoDaVelha(jogador, tabuleiro);

            bool inicializacaoVariaveis = true;

            while (true)
            {

                if (inicializacaoVariaveis)
                {

                    marcador = "";
                    haVencedor = false;
                    vencedor = "";
                    jogador = "1";
                    qtdeMarcacoes = 0;

                    tabuleiro[0, 0] = "1";
                    tabuleiro[0, 1] = "2";
                    tabuleiro[0, 2] = "3";
                    tabuleiro[1, 0] = "4";
                    tabuleiro[1, 1] = "5";
                    tabuleiro[1, 2] = "6";
                    tabuleiro[2, 0] = "7";
                    tabuleiro[2, 1] = "8";
                    tabuleiro[2, 2] = "9";

                    MostrarJogoDaVelha(jogador, tabuleiro);

                    inicializacaoVariaveis = false;

                }
               
                string numeroEscolhido = Console.ReadLine();
                bool numeroEncontrado = false;
                Console.Clear();

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (numeroEscolhido == tabuleiro[i, j])
                        {
                            qtdeMarcacoes++;

                            if (jogador == "1")
                            {
                                marcador = "X";
                                jogador = "2";
                            }
                            else if (jogador == "2")
                            {
                                marcador = "O";
                                jogador = "1";
                            }

                            tabuleiro[i, j] = marcador;
                            numeroEncontrado = true;

                            vencedor = VerificarGanhador(tabuleiro);

                            if (vencedor != "") haVencedor = true;

                            if (qtdeMarcacoes == 9) houveEmpate = true;

                            break;
                        }

                    }
                    if (numeroEncontrado) break;
                }
                
                MostrarJogoDaVelha(jogador, tabuleiro);


                if (haVencedor)
                {

                    houvePartida = true;

                    if (vencedor == "1") vitoriasJogador1++;
                    else if (vencedor == "2") vitoriasJogador2++;

                    Console.Clear();
                    Console.WriteLine($"Parabéns, jogador {vencedor}. Você venceu!\n");
                    Console.Write("Digite qualquer número para jogar outra partida ou zero para encerrar o programa: ");
                    string opcao = Console.ReadLine();

                    if (opcao == "0") sairPrograma = true;
                    else inicializacaoVariaveis = true;
                    
                }

                if(houveEmpate)
                {

                    empates++;

                    houvePartida = true;
                    houveEmpate = false;

                    Console.Clear();
                    Console.WriteLine("Houve empate!\n");
                    Console.Write("Digite qualquer número para jogar outra partida ou zero para encerrar o programa: ");

                    string opcao = Console.ReadLine();

                    if (opcao == "0") sairPrograma = true;
                    else inicializacaoVariaveis = true;

                }

                if (houvePartida)
                {
                    quantidadePartidas++;
                    houvePartida = false;
                }

                if (sairPrograma) break;

            }
            
            SalvarDadosJogo(vitoriasJogador1, vitoriasJogador2, empates, quantidadePartidas);

        }

        static void Main(string[] args)
        {
            MostrarHistoricoPartidasAnteriores();
            Jogar();
        }

    }

}