using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculatrice
{
    class Program
    {


        static void Main(string[] args)
        {


            decimal resultat;

            Console.WriteLine("Veuillez entrer l'opération à effectuer");
            string operation = Console.ReadLine();
            //test
            //string operation = "(-3*4)+5*(4-5)";
            //string operation = "(12,4*3,2+13,4/3,8*6,7)*5,9-(-26,2/2,3+16,7)";
            //resultat avec float : 368,4553
            //resultat plus precis avec decimal : 368,1985
            //string operation = "";
            Console.WriteLine(operation);
            //teste si c'est une factorielle
            if (operation.Contains("!"))
            {
                long resultatLong = Factorielle(operation);
             
                Console.WriteLine("Le résultat de l'opération est : " + resultatLong);

               
            }
            else
            {
                // Pas une factorielle, calcul "normal"

                if(operation.Contains("("))
                {
                    resultat = CalculAvecParenthese(operation);
                    
                }
                else
                {
                    resultat = CalculSansParenthese(operation);
                    
                }
                Console.WriteLine("Le résultat de l'opération est : " + resultat);
            }
            Console.ReadKey();
        }

        private static decimal CalculSansParenthese(string operation)
        {
            // conversion de la chaine de caractere en tableau de char pour pouvoir tester chaque caractere

            int i;

            char[] tabCharOperation;

            int nbOperande;
            int nbOperateur;

            int j = 0;
            int k;
            int l;

            int cptOperationFinies = 0;

            // constante pour mettre une valeur que l'utilisateur ne va pas saisir que l'on va entrer dans le tableau
            // quand une operande n'est plus utile car nous avons effectué une opération intermédiaire...
            const decimal MAX = 9999999999;

            tabCharOperation = operation.ToCharArray();


            decimal[] tabOperande = new decimal[tabCharOperation.Length];
            char[] tabOperateur = new char[tabCharOperation.Length];
            bool[] tabBoolTryParse = new bool[tabCharOperation.Length];
            decimal[] tabResultatTryParse = new decimal[tabCharOperation.Length];

            for (i = 0; i < tabCharOperation.Length; i++)
            {
                // stocke dans un tableau tabBoolTryParse les resultats (bool) (valeur de retour) des TryParse de chaque caractere
                // et stocke dans un autre tableau tabResultatTryParse les resultats (decimal) des TryParse de chaque caractere
                tabBoolTryParse[i] = decimal.TryParse(tabCharOperation[i].ToString(), out tabResultatTryParse[i]);
                //Console.WriteLine("tabBoolTryParse[" + i + "]" + tabBoolTryParse[i]);
                //Console.WriteLine("tabResultatTryParse[" + i + "]" + tabResultatTryParse[i]); 
            }

            //reinitialisation de i
            i = 0;
            do
            {
                // remplissage du tableau tabOperande avec les operandes trouves dans tabCharOperation
                while (i < tabCharOperation.Length && (tabBoolTryParse[i] == true || tabCharOperation[i] == ','))
                {
                    if (tabCharOperation[i] == ',')
                    {

                    }
                    else
                    {
                        if (i > 0 && tabCharOperation[i - 1] == ',')
                        {
                            tabOperande[j] = decimal.Parse(tabOperande[j].ToString() + "," + (tabCharOperation[i]).ToString());
                        }
                        else
                        {
                            if ((i > 1 && (tabCharOperation[i - 1] == '-' && (tabCharOperation[i - 2] == '+' || tabCharOperation[i - 2] == '-' || tabCharOperation[i - 2] == '*' || tabCharOperation[i - 2] == '/'))) || (i == 1 && tabCharOperation[i - 1] == '-'))
                            {
                                // on a un nombre negatif
                                tabOperande[j] = decimal.Parse( "-" + (tabCharOperation[i]).ToString());
                            }
                            else
                            {
                                tabOperande[j] = decimal.Parse(tabOperande[j].ToString() + (tabCharOperation[i]).ToString());
                            }
                        }
                    }
                   
                    i++;
                }
                Console.WriteLine("tabOperande[" + j + "]" + tabOperande[j]);

                // remplissage du tableau tabOperateur avec les operateurs trouves dans tabCharOperation
                if (i < tabCharOperation.Length)
                {
                    if ((i > 0 && (tabCharOperation[i] == '-' && (tabCharOperation[i - 1] == '+' || tabCharOperation[i - 1] == '-' || tabCharOperation[i - 1] == '*' || tabCharOperation[i - 1] == '/'))) || (i == 0 && tabCharOperation[i] == '-'))
                    {
                        // on a un nombre negatif
                        Console.WriteLine("On a un nombre negatif");
                        // Pour que le compteur n'incremente pas avec le j++ qui vient ensuite.
                        j--;
                    }
                    else
                    {
                        tabOperateur[j] = tabCharOperation[i];

                        Console.WriteLine("tabOperateur[" + j + "]" + tabOperateur[j]);
                    }
                }

                j++;

                i++;

            } while (i < tabCharOperation.Length);

            nbOperande = j + 1;
            nbOperateur = j - 1;


            int nbOpMultiDivi = 0;

            // recherche du nombre de multiplication et de division à effectuer dans l'operation

            for (int o = 0; o < tabOperateur.Length; o++)
            {
                if (tabOperateur[o] == '*' || tabOperateur[o] == '/')
                    nbOpMultiDivi++;
            }

            int cptOpMultiDivi = nbOpMultiDivi;

            do
            {
                k = 0;
                do
                {

                    if ((tabOperateur[k] == '*' || tabOperateur[k] == '/') && cptOpMultiDivi > 0)
                    {
                        // Cherche l'indice de la derniere operande qui n'est pas egale a max, 
                        // pour pouvoir effectuer l'operation entre la nouvelle operande (resultat de l'operation intermediaire)
                        // et  tabOperande[k + 1]
                        for (l = k; l > 0 && tabOperande[l] == MAX; l--)
                        {

                        }

                        tabOperande[l] = calculEntre2OPerandes(tabOperande[l], tabOperande[k + 1], tabOperateur[k]);

                        Console.WriteLine("tabOperande[" + (l) + "]" + tabOperande[l]);

                        // "effacement" de tabOperateur[k] car l'operation a ete effectuee
                        tabOperateur[k] = ' ';
                        cptOperationFinies++;


                        // "effacement" de tabOperande[k + 1] car l'operation a ete effectuee
                        tabOperande[k + 1] = MAX;

                        // REinitalisation de k
                        k = -1;
                        cptOpMultiDivi--;
                    }


                    else
                    {
                        // Il n'y a plus de multiplication ou de division à faire

                        if ((tabOperateur[k] == '+' || tabOperateur[k] == '-') && cptOpMultiDivi == 0)
                        {
                            Console.WriteLine("Il n'y a plus de multiplication ou de division à faire");
                            // Cherche l'indice de la derniere operande qui n'est pas egale a max, 
                            // pour pouvoir effectuer l'operation entre la nouvelle operande (resultat de l'operation intermediaire)
                            // et  tabOperande[k + 1]

                            for (l = k; l > 0 && tabOperande[l] == MAX; l--)
                            {

                            }


                            tabOperande[l] = calculEntre2OPerandes(tabOperande[l], tabOperande[k + 1], tabOperateur[k]);

                            Console.WriteLine("tabOperande[" + (l) + "]" + tabOperande[l]);

                            // "effacement" de tabOperateur[k] car l'operation a ete effectuee
                            tabOperateur[k] = ' ';
                            cptOperationFinies++;

                            // "effacement" de tabOperande[k + 1] car l'operation a ete effectuee
                            tabOperande[k + 1] = MAX;

                            // REinitalisation de k
                            k = -1;

                        }
                    }


                    k++;


                } while (k < nbOperateur);
                Console.WriteLine("reinitialisation de k");
                // Reinitilisation de k pour pouvoir reparcourir le tabOperande et effectuer les operation qu'il
                // reste encore à effectuer
                k = 0;


            } while (cptOperationFinies <= nbOperateur - 1);

            return tabOperande[0];

            
        }

        private static decimal CalculAvecParenthese(string operation)
        {
            bool boolVerifParenthese = VerificationParenthese(operation);
            Console.WriteLine("boolVerifParenthese : " + boolVerifParenthese);
            decimal resultatParenthese;

            if (boolVerifParenthese == false)
            {
                Console.WriteLine("Veuillez entrer une operation avec un nombre de parentheses cohérent");
                resultatParenthese = 0;
            }
            else
            {
                // Il y a le bon nombre de parenthèses
                int indexDerniereParentOuvrante;
                int indexPremiereParentFermante;
                int longueurExpressionParentProfonde;
                string expressionParentProfonde;
                decimal resultatExpressionParentProfonde;
                
                //cherche la premiere operation intermediaire a effectuer, celle avec les parentheses les plus profondes
                do
                {


                    indexDerniereParentOuvrante = operation.LastIndexOf('(');
                    indexPremiereParentFermante = operation.IndexOf(')');
                    longueurExpressionParentProfonde = indexPremiereParentFermante - indexDerniereParentOuvrante;

                    if (longueurExpressionParentProfonde < 0)
                    {
                        string debutExpression;
                        // on n'a pas pris la bonne paire de parenthese car il y a plusieurs parentheses du mm degre
                        debutExpression = operation.Substring(0, indexPremiereParentFermante);
                        indexDerniereParentOuvrante = debutExpression.LastIndexOf('(');
                        longueurExpressionParentProfonde = indexPremiereParentFermante - indexDerniereParentOuvrante;
                    }
                    expressionParentProfonde = operation.Substring(indexDerniereParentOuvrante + 1, longueurExpressionParentProfonde - 1);
                    //On ne prend pas la parenthese ouvrante d'ou le +1, et on ne prend pas la parenthese fermante d'ou le - 1.
                    Console.WriteLine(expressionParentProfonde);
                    resultatExpressionParentProfonde = CalculSansParenthese(expressionParentProfonde);
                    Console.WriteLine("resultatExpressionParentProfonde : " + resultatExpressionParentProfonde);

                    //Remplacement  (expressionParentProfonde) par resultatExpressionParentProfonde
                    operation = operation.Remove(indexDerniereParentOuvrante, longueurExpressionParentProfonde + 1);
                    //Console.WriteLine("operation apres remove : " + operation);
                    operation = operation.Insert(indexDerniereParentOuvrante, resultatExpressionParentProfonde.ToString());
                    Console.WriteLine("operation apres insert : " + operation);

                } while (operation.Contains('('));

                Console.WriteLine("l'operation ne contient plus de parenthese");
                resultatParenthese = CalculSansParenthese(operation);

            }

            return resultatParenthese;
        }

        private static bool VerificationParenthese(string operation)
        {
            char[] tabCharOperation;

            tabCharOperation = operation.ToCharArray();
            int nbParentOuvrantes = 0;
            int nbParentFermantes = 0;

            for (int i = 0; i < tabCharOperation.Length; i++)
            {
                switch (tabCharOperation[i])
                {
                    case '(':
                        nbParentOuvrantes++;
                        break;

                    case ')':
                        nbParentFermantes++;
                        break;

                    default:
                        break;
                }
                
            }
            if (nbParentOuvrantes == nbParentFermantes)
                return true;
            else
                return false;
            
        }

        private static long Factorielle(string operation)
        {
            long resultatTryParse;
            bool boolTryParse;
            long resultatFact = 0;

            int indexFact = operation.Length - 1;
            operation = operation.Remove(indexFact);
            Console.WriteLine(operation);

            boolTryParse = long.TryParse(operation, out resultatTryParse);

            if (boolTryParse == false)
            {
                Console.WriteLine("Veuillez saisir uniquement un entier pour une factorielle");
            }
            else
            {
                resultatFact = calculFact(resultatTryParse);
            }

            
            return resultatFact;
        }

        private static long calculFact(long nombre)
        {
            if (nombre != 1)
            {
                return (nombre * calculFact(nombre - 1));
            }
            else
            {
                return 1;
            }
        }

        private static decimal calculEntre2OPerandes(decimal operande1, decimal operande2, char operateur)
        {
            decimal resultat;

            switch (operateur)
            {
                case '+':
                    resultat = operande1 + operande2;
                    break;

                case '-':
                    resultat = operande1 - operande2;
                    break;

                case '*':
                    resultat = operande1 * operande2;
                    break;

                case '/':
                    if (operande2 != 0)
                    {
                        resultat = operande1 / operande2;
                    }
                    else
                    {
                        resultat = 0;
                        Console.WriteLine("Vous ne pouvez pas diviser par 0");
                    }
                    break;

                default:
                    resultat = 0;
                    break;
            }
            return resultat;
            
        }
    }
}
