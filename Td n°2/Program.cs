using ReedSolomon;
using System;

namespace Td_n_2
{
    public class Program
    {
        /// <summary>
        /// Menu du programme
        /// </summary>
        /// <returns> Retourne le mode choisi </returns>
        public static int Menu()
        {
            int choixdumode = 0;
            while (choixdumode != 1 && choixdumode != 2 && choixdumode != 3 && choixdumode != 4 && choixdumode != 5 && choixdumode != 6)
            {
                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("                                                   Menu Image                                                         ");
                Console.WriteLine("                                                                                                                      ");
                Console.WriteLine("||       1. Modifications simples de l'image                                                                        ||");
                Console.WriteLine("||       2. Filtres par convolution                                                                                 ||");
                Console.WriteLine("||       3. Histogramme                                                                                             ||");
                Console.WriteLine("||       4. Fractale                                                                                                ||");
                Console.WriteLine("||       5. Stéganographie                                                                                          ||");
                Console.WriteLine("||       6. QR code                                                                                                 ||");
                Console.WriteLine("                                                                                                                      ");
                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("                                                                                                                      ");
                Console.WriteLine("                                                                                                                      ");
                Console.WriteLine("-> ?   ");
                choixdumode = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
            }
            return choixdumode;
        }
        public static void Main()
        {
            bool quitter = false;

            while (quitter == false)
            {
                Console.Clear();
                Console.WriteLine("Indiquez le nom de l'image à modifier ainsi que son extension : ");
                string emplacement = Console.ReadLine();

                MyImage image = new MyImage(emplacement);

                switch (Menu())
                {
                    case 1: // Modifications simples
                        Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("                                                   Modifications simples                                              ");
                        Console.WriteLine("                                                                                                                      ");
                        Console.WriteLine("||       1. Agrandissement                                                                                          ||");
                        Console.WriteLine("||       2. Réduction                                                                                               ||");
                        Console.WriteLine("||       3. Passage en noir et blanc                                                                                ||");
                        Console.WriteLine("||       4. Passage en nuances de gris                                                                              ||");
                        Console.WriteLine("||       5. Rotation                                                                                                ||");
                        Console.WriteLine("||       6. Effet mirroir                                                                                           ||");
                        Console.WriteLine("||       7. Négatif                                                                                                 ||");
                        Console.WriteLine("||       8. Luminosité                                                                                              ||");
                        Console.WriteLine("                                                                                                                      ");
                        Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("                                                                                                                      ");
                        Console.WriteLine("                                                                                                                      ");
                        Console.WriteLine("-> ?   ");
                        int mode = Convert.ToInt32(Console.ReadLine());
                        Console.Clear();

                        switch (mode)
                        {
                            case 1: // Agrandissement
                                image.Dimension(0);
                                break;

                            case 2: // Réduction
                                image.Dimension(1);
                                break;

                            case 3: // Noir & Blanc
                                int[] tmode = { 0, 0 };
                                int valeur = -1;
                                while (valeur < 0 && valeur > 255)
                                {
                                    Console.WriteLine("Indiquez la valeur de basculement entre noir et blanc (par défaut à 128) comprise entre 0 et 255 : ");
                                    valeur = Convert.ToInt32(Console.ReadLine());
                                }
                                image.NoirBlancGris(tmode);
                                break;

                            case 4: // Nuance de Gris
                                int[] tmode2 = { 1, 0 };
                                int valeur2 = -1;
                                while (valeur2 < 0 || valeur2 > 2)
                                {
                                    Console.WriteLine("Indiquez le mode souhaité (1 - Version claire, 2 - Version moyenne, 3 - Version luminosité : ");
                                    valeur2 = Convert.ToInt32(Console.ReadLine());
                                }
                                image.NoirBlancGris(tmode2);
                                break;

                            case 5: // Rotation
                                int valeur3 = -1;
                                while (valeur3 != 90 && valeur3 != 180 && valeur3 != 270)
                                {
                                    Console.WriteLine("Indiquez la rotation souhaitée entre 90, 180, et 270 degrés : ");
                                    valeur3 = Convert.ToInt32(Console.ReadLine());
                                }
                                image.Rotation(valeur3);
                                break;

                            case 6: // Effet mirroir
                                string valeur4 = "";
                                while (valeur4 != "horizontal" && valeur4 != "vertical")
                                {
                                    Console.WriteLine("Indiquez le sens de l'effet, horizontal ou vertical : ");
                                    valeur4 = Console.ReadLine().ToLower();
                                }
                                image.EffetMiroir(valeur4);
                                break;

                            case 7:
                                image.Negatif();
                                break;

                            case 8:
                                Console.WriteLine("Choisissez le pourcentage de luminosité -/+ : ");
                                int pourcent = Convert.ToInt32(Console.ReadLine());
                                image.Luminosité(pourcent);
                                break;

                            default:
                                Console.WriteLine("Veuillez indiquer un nombre entre 1 et 6");
                                break;
                        }
                        break;

                    case 2: // Filtres de convolution
                        Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("                                                 Filtres de convolution                                               ");
                        Console.WriteLine("                                                                                                                      ");
                        Console.WriteLine("||       1. Filtre de flou                                                                                          ||");
                        Console.WriteLine("||       2. Repoussage                                                                                              ||");
                        Console.WriteLine("||       3. Renforcement de contours                                                                                ||");
                        Console.WriteLine("||       4. Détection de contours                                                                                   ||");
                        Console.WriteLine("||       5. Réhausseur de contours                                                                                  ||");
                        Console.WriteLine("                                                                                                                      ");
                        Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("                                                                                                                      ");
                        Console.WriteLine("                                                                                                                      ");
                        Console.WriteLine("-> ?   ");
                        int mode2 = Convert.ToInt32(Console.ReadLine());
                        Console.Clear();

                        switch (mode2)
                        {
                            case 1: // Flou
                                image.Convolution(1, 1);
                                break;

                            case 2: // Repoussage
                                Console.WriteLine("-> Voulez rentrer un diviseur particulier : ");
                                int div = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("-> Voulez rentrer un décalage particulier : ");
                                int dec = Convert.ToInt32(Console.ReadLine());
                                image.Convolution(2, div, dec);
                                break;

                            case 3: // Renforcement de contours
                                image.Convolution(3, 5, 1);
                                break;

                            case 4: // Détection de contours
                                image.Convolution(4, 1, 128);
                                int[] tempmode = { 0, 0 };
                                image.NoirBlancGris(tempmode);
                                break;

                            case 5: // Réhausseur de contours
                                Console.WriteLine("-> Voulez rentrer un diviseur particulier : ");
                                int div2 = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("-> Voulez rentrer un décalage particulier : ");
                                int dec2 = Convert.ToInt32(Console.ReadLine());
                                image.Convolution(5, div2, dec2);
                                break;

                            default:
                                Console.WriteLine("Veuillez indiquer un nombre entre 1 et 5");
                                break;
                        }
                        break;

                    case 3: // Histogramme
                        image.Histogramme();
                        break;

                    case 4: // Fractale de Mandelbrot
                        image.FractaleMandelbrot();
                        break;

                    case 5: // Stéganographie
                        Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("                                                 Stéganographie                                                       ");
                        Console.WriteLine("                                                                                                                      ");
                        Console.WriteLine("||       1. Codage                                                                                                  ||");
                        Console.WriteLine("||       2. Décodage                                                                                                ||");
                        Console.WriteLine("                                                                                                                      ");
                        Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("                                                                                                                      ");
                        Console.WriteLine("                                                                                                                      ");
                        Console.WriteLine("-> ?   ");
                        int mode3 = Convert.ToInt32(Console.ReadLine());
                        Console.Clear();

                        if (mode3 == 1)
                        {
                            Console.WriteLine("Indiquez le nom de l'image à cacher ainsi que son extension : ");
                            string emplacement2 = Console.ReadLine();

                            MyImage aDissimuler = new MyImage(emplacement2);

                            image.SteganoEncod(aDissimuler);
                        }
                        if (mode3 == 2)
                        {
                            image.SteganoDecod2();
                        }
                        break;

                    case 6: //QrCode
                        QrCode Image = new QrCode();

                        Console.WriteLine("Entrez le message à inscrire dans le QR code : ");
                        Console.WriteLine("Taille comprise inférieure ou égale à 47 caractères...");
                        Console.WriteLine("Caractères autorisés : alphanumériques & $% *+-./:");
                        string objet = Console.ReadLine();
                        //string objet = "HELLO WORLD";

                        byte[] Chaine = Image.InfosQRCode(objet);

                        byte[] Correction = { 209, 239, 196, 207, 78, 195, 109 };
                        //byte[] Correction = ReedSolomonAlgorithm.Encode(Image.InfosQRCode2(objet), 7, ErrorCorrectionCodeType.QRCode);

                        byte[] Info = new byte[Chaine.Length + 7 * 8];
                        for (int i = 0; i < Chaine.Length; i++)
                        {
                            Info[i] = Chaine[i];
                        }

                        string OctetsCorrection = "";
                        for (int i = 0; i < Correction.Length; i++)
                        {
                            byte[] octet = Image.Convertir_Int_to_Binaire(Correction[i], 8);
                            for (int j = 0; j < 8; j++)
                            {
                                OctetsCorrection += octet[j];
                            }
                        }

                        string StringDonnées = "";
                        for (int i = 0; i < Chaine.Length; i++)
                        {
                            StringDonnées += Chaine[i];
                        }
                        StringDonnées += OctetsCorrection;
                        for (int i = 0; i < StringDonnées.Length; i++)
                        {
                            if (StringDonnées[i] == '0')
                            {
                                Info[i] = 0;
                            }
                            else
                            {
                                Info[i] = 1;
                            }
                        }

                        Pixel[,] AP = Image.CréationQR();
                        Image.AgrandissementCode(Image.Masque(Info, AP));

                        break;

                    default:
                        Console.WriteLine("Veuillez rentrer un chiffre entre 1 et 6");
                        break;
                }
                Console.WriteLine("Voulez-vous executer une nouvelle fois le programme ?");
                Console.WriteLine("y/n");
                char temp = Convert.ToChar(Console.ReadLine());
                if (temp == 'n')
                {
                    quitter = true;
                }
            }            
        }
    }
}
