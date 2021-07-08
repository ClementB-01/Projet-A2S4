using System;
using System.Collections.Generic;
using System.Text;

namespace Td_n_2
{
    class QrCode
    {
        /// <summary>
        /// Conversion d'un entier de taille donnée en un tableau de byte
        /// </summary>
        /// <param name="obj"> Entier à convertir </param>
        /// <param name="taille"> Taille de l'entier considéré </param>
        /// <returns> Tableau de byte équivalent à l'entier </returns>
        public byte[] Convertir_Int_to_Binaire(int obj, int taille)
        {
            byte[] bit = new byte[taille];
            int obj2 = 0;
            for (int i = taille - 1; i > -1; i--)
            {
                obj2 = Convert.ToInt32(Math.Truncate(obj / (Math.Pow(2, i))));
                bit[taille - 1 - i] = Convert.ToByte(obj2);
                obj = Convert.ToInt32(obj % (Math.Pow(2, i)));
            }
            return bit;
        }

        /// <summary>
        /// Encode les informations qui doivent être codés par les pixels
        /// </summary>
        /// <param name="objet"> Chaîne de caractère que le QR code doit signifier </param>
        /// <returns> Tableau de byte équivalent à la chaîne de caractère entrée </returns>
        public byte[] InfosQRCode2(string objet)
        {
            byte[] TailleObjet9bits = Convertir_Int_to_Binaire(objet.Length, 9);
            int[] TableauObjet = new int[objet.Length];
            string Caractères = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:";
            //Traduction objet en binaire

            for (int i = 0; i < objet.Length; i++)
            {
                char Lettre = objet[i];
                for (int j = 0; j < Caractères.Length; j++)
                {
                    if (Lettre == Caractères[j])
                    {
                        TableauObjet[i] = j;
                    }
                }
            }
            //string DébutChaine = "0010";
            string Chaine = "";
            //string TerminaisonChaine = "0000";
            int Chaine2Lettres = 0;
            byte[] Tampon;
            for (int i = 0; i < 9; i++)
            {
                Chaine = Chaine + TailleObjet9bits[i];
            }

            if (objet.Length % 2 == 0)
            {
                for (int i = 0; i < objet.Length; i = i + 2)
                {
                    Chaine2Lettres = Convert.ToInt32(45 * TableauObjet[i] + (TableauObjet[i + 1]));
                    Tampon = Convertir_Int_to_Binaire(Chaine2Lettres, 11);
                    for (int j = 0; j < 11; j++)
                    {
                        Chaine = Chaine + Tampon[j];
                    }
                }
            }
            else
            {
                for (int i = 0; i < objet.Length; i = i + 2)
                {
                    if (i == objet.Length - 1)
                    {
                        Chaine2Lettres = Convert.ToInt32(TableauObjet[i]);
                        Tampon = Convertir_Int_to_Binaire(Chaine2Lettres, 6);
                        for (int j = 0; j < 6; j++)
                        {
                            Chaine = Chaine + Tampon[j];
                        }
                    }
                    else
                    {
                        Chaine2Lettres = Convert.ToInt32(45 * TableauObjet[i] + (TableauObjet[i + 1]));
                        Tampon = Convertir_Int_to_Binaire(Chaine2Lettres, 11);
                        for (int j = 0; j < 11; j++)
                        {
                            Chaine = Chaine + Tampon[j];
                        }
                    }
                }
            }

            //Chaine = Chaine + TerminaisonChaine;
            string Nombre236 = "11101100";
            string Nombre17 = "00010001";

            while (Chaine.Length % 8 != 0)
            {
                Chaine = Chaine + "0";
            }
            int Iteration = (152 - Chaine.Length) / 8;
            int x = 0;

            for (int i = 0; i < Iteration; i++)
            {
                if (x % 2 == 0)
                {
                    Chaine = Chaine + Nombre236;
                }
                else
                {
                    Chaine = Chaine + Nombre17;
                }
                x++;
            }

            Byte[] objetBites = new byte[Chaine.Length];

            for (int i = 0; i < Chaine.Length; i++)
            {
                if (Chaine[i] == '0')
                {
                    objetBites[i] = 0;
                }
                else
                {
                    objetBites[i] = 1;
                }
            }
            return objetBites;
        }

        /// <summary>
        /// Encode les informations qui doivent être codés par les pixels
        /// </summary>
        /// <param name="objet"> Chaîne de caractère que le QR code doit signifier </param>
        /// <returns> Tableau de byte équivalent à la chaîne de caractère entrée </returns>
        public byte[] InfosQRCode(string objet)
        {
            byte[] TailleObjet9bits = Convertir_Int_to_Binaire(objet.Length, 9);
            int[] TableauObjet = new int[objet.Length];
            string Caractères = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:";
            //Traduction objet en binaire

            for (int i = 0; i < objet.Length; i++)
            {
                char Lettre = objet[i];
                for (int j = 0; j < Caractères.Length; j++)
                {
                    if (Lettre == Caractères[j])
                    {
                        TableauObjet[i] = j;
                    }
                }
            }
            string DébutChaine = "0010";
            string Chaine = DébutChaine;
            string TerminaisonChaine = "0000";
            int Chaine2Lettres = 0;
            byte[] Tampon;
            for (int i = 0; i < 9; i++)
            {
                Chaine = Chaine + TailleObjet9bits[i];
            }

            if (objet.Length % 2 == 0)
            {
                for (int i = 0; i < objet.Length; i = i + 2)
                {
                    Chaine2Lettres = Convert.ToInt32(45 * TableauObjet[i] + (TableauObjet[i + 1]));
                    Tampon = Convertir_Int_to_Binaire(Chaine2Lettres, 11);
                    for (int j = 0; j < 11; j++)
                    {
                        Chaine = Chaine + Tampon[j];
                    }
                }
            }
            else
            {
                for (int i = 0; i < objet.Length; i = i + 2)
                {
                    if (i == objet.Length - 1)
                    {
                        Chaine2Lettres = Convert.ToInt32(TableauObjet[i]);
                        Tampon = Convertir_Int_to_Binaire(Chaine2Lettres, 6);
                        for (int j = 0; j < 6; j++)
                        {
                            Chaine = Chaine + Tampon[j];
                        }
                    }
                    else
                    {
                        Chaine2Lettres = Convert.ToInt32(45 * TableauObjet[i] + (TableauObjet[i + 1]));
                        Tampon = Convertir_Int_to_Binaire(Chaine2Lettres, 11);
                        for (int j = 0; j < 11; j++)
                        {
                            Chaine = Chaine + Tampon[j];
                        }
                    }
                }
            }

            Chaine = Chaine + TerminaisonChaine;
            string Nombre236 = "11101100";
            string Nombre17 = "00010001";

            while (Chaine.Length % 8 != 0)
            {
                Chaine = Chaine + "0";
            }
            int Iteration = (152 - Chaine.Length) / 8;
            int x = 0;

            for (int i = 0; i < Iteration; i++)
            {
                if (x % 2 == 0)
                {
                    Chaine = Chaine + Nombre236;
                }
                else
                {
                    Chaine = Chaine + Nombre17;
                }
                x++;
            }
            Byte[] objetBites = new byte[Chaine.Length];

            for (int i = 0; i < Chaine.Length; i++)
            {
                if (Chaine[i] == '0')
                {
                    objetBites[i] = 0;
                }
                else
                {
                    objetBites[i] = 1;
                }
            }
            return objetBites;
        }

        /// <summary>
        /// Méthode qui crée la matrice support du QR code qui permettra d'afficher l'image par la suite
        /// </summary>
        /// <returns> Matrice de pixels correspondante </returns>
        public Pixel[,] CréationQR()
        {
            int[] b = { 0, 0, 0 };
            int[] w = { 255, 255, 255 };
            Pixel[,] AP = new Pixel[21, 21];
            Pixel B = new Pixel(b);
            Pixel W = new Pixel(w);

            for (int i = 0; i < 21; i++)
            {
                for (int j = 0; j < 21; j++)
                {
                    AP[i, j] = W;
                }
            }

            for (int i = 0; i < 7; i++)
            {
                AP[0, i] = B;
                AP[6, i] = B;
                AP[i, 0] = B;
                AP[i, 6] = B;

                AP[14, i] = B;
                AP[20, i] = B;
                AP[14 + i, 6] = B;
                AP[14 + i, 0] = B;

                AP[14, 14 + i] = B;
                AP[20, 14 + i] = B;
                AP[14 + i, 14] = B;
                AP[14 + i, 20] = B;

            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    AP[i + 2, j + 2] = B;
                    AP[i + 16, j + 2] = B;
                    AP[i + 16, j + 16] = B;
                }
            }

            AP[8, 6] = B;
            AP[10, 6] = B;
            AP[12, 6] = B;
            AP[14, 8] = B;

            AP[14, 10] = B;
            AP[14, 12] = B;
            AP[7, 8] = B;
            AP[12, 0] = B;

            AP[12, 1] = B;
            AP[12, 2] = B;
            AP[12, 4] = B;
            AP[12, 5] = B;

            AP[12, 7] = B;
            AP[12, 8] = B;
            AP[13, 8] = B;
            AP[18, 8] = B;

            AP[0, 8] = B;
            AP[1, 8] = B;
            AP[2, 8] = B;
            AP[4, 8] = B;

            AP[5, 8] = B;
            AP[6, 8] = B;
            AP[12, 13] = B;
            AP[12, 18] = B;
            AP[12, 14] = B;

            return AP;
        }

        /// <summary>
        /// Méthode qui permet de prendre en compte l'étape de masquage du QR code
        /// </summary>
        /// <param name="Info"> Tableau de byte comportant l'intégralité des données du QR code </param>
        /// <param name="CréationQR"> Matrice support du QR code </param>
        /// <returns></returns>
        public Pixel[,] Masque(byte[] Info, Pixel[,] CréationQR)
        {
            int[] b = { 0, 0, 0 };
            Pixel B = new Pixel(b);

            int x = 0;
            // Colonne 1
            for (int i = 0; i < 12; i++)
            {
                if ((i + 20) % 2 == 1)
                {
                    if (Info[x] == 1)
                    {
                        CréationQR[i, 20] = B;
                    }
                }
                if ((i + 20) % 2 == 0)
                {
                    if (Info[x] == 0)
                    {
                        CréationQR[i, 20] = B;
                    }
                }
                if ((i + 19) % 2 == 1)
                {
                    if (Info[x + 1] == 1)
                    {
                        CréationQR[i, 19] = B;
                    }
                }
                if ((i + 19) % 2 == 0)
                {
                    if (Info[x + 1] == 0)
                    {
                        CréationQR[i, 19] = B;
                    }
                }
                x += 2;
            }
            // Colonne2

            for (int i = 11; i > -1; i--)
            {
                if ((i + 18) % 2 == 1)
                {
                    if (Info[x] == 1)
                    {
                        CréationQR[i, 18] = B;
                    }
                }
                if ((i + 18) % 2 == 0)
                {
                    if (Info[x] == 0)
                    {
                        CréationQR[i, 18] = B;
                    }
                }
                if ((i + 17) % 2 == 1)
                {
                    if (Info[x + 1] == 1)
                    {
                        CréationQR[i, 17] = B;
                    }
                }
                if ((i + 17) % 2 == 0)
                {
                    if (Info[x + 1] == 0)
                    {
                        CréationQR[i, 17] = B;
                    }
                }
                x += 2;
            }
            // colonne 3
            for (int i = 0; i < 12; i++)
            {
                if ((i + 16) % 2 == 1)
                {
                    if (Info[x] == 1)
                    {
                        CréationQR[i, 16] = B;
                    }
                }
                if ((i + 16) % 2 == 0)
                {
                    if (Info[x] == 0)
                    {
                        CréationQR[i, 16] = B;
                    }
                }
                if ((i + 15) % 2 == 1)
                {
                    if (Info[x + 1] == 1)
                    {
                        CréationQR[i, 15] = B;
                    }
                }
                if ((i + 15) % 2 == 0)
                {
                    if (Info[x + 1] == 0)
                    {
                        CréationQR[i, 15] = B;
                    }
                }
                x += 2;
            }
            for (int i = 11; i > -1; i--)
            {
                if ((i + 14) % 2 == 1)
                {
                    if (Info[x] == 1)
                    {
                        CréationQR[i, 14] = B;
                    }
                }
                if ((i + 14) % 2 == 0)
                {
                    if (Info[x] == 0)
                    {
                        CréationQR[i, 14] = B;
                    }
                }
                if ((i + 13) % 2 == 1)
                {
                    if (Info[x + 1] == 1)
                    {
                        CréationQR[i, 13] = B;
                    }
                }
                if ((i + 13) % 2 == 0)
                {
                    if (Info[x + 1] == 0)
                    {
                        CréationQR[i, 13] = B;
                    }
                }
                x += 2;
            }
            for (int i = 0; i < 14; i++)
            {
                if ((i + 12) % 2 == 1)
                {
                    if (Info[x] == 1)
                    {
                        CréationQR[i, 12] = B;
                    }
                }
                if ((i + 12) % 2 == 0)
                {
                    if (Info[x] == 0)
                    {
                        CréationQR[i, 12] = B;
                    }
                }
                if ((i + 11) % 2 == 1)
                {
                    if (Info[x + 1] == 1)
                    {
                        CréationQR[i, 11] = B;
                    }
                }
                if ((i + 11) % 2 == 0)
                {
                    if (Info[x + 1] == 0)
                    {
                        CréationQR[i, 11] = B;
                    }
                }
                x += 2;
            }
            for (int i = 15; i < 21; i++)
            {
                if ((i + 12) % 2 == 1)
                {
                    if (Info[x] == 1)
                    {
                        CréationQR[i, 12] = B;
                    }
                }
                if ((i + 12) % 2 == 0)
                {
                    if (Info[x] == 0)
                    {
                        CréationQR[i, 12] = B;
                    }
                }
                if ((i + 11) % 2 == 1)
                {
                    if (Info[x + 1] == 1)
                    {
                        CréationQR[i, 11] = B;
                    }
                }
                if ((i + 11) % 2 == 0)
                {
                    if (Info[x + 1] == 0)
                    {
                        CréationQR[i, 11] = B;
                    }
                }
                x += 2;
            }
            for (int i = 20; i > 14; i--)
            {
                if ((i + 10) % 2 == 1)
                {
                    if (Info[x] == 1)
                    {
                        CréationQR[i, 10] = B;
                    }
                }
                if ((i + 10) % 2 == 0)
                {
                    if (Info[x] == 0)
                    {
                        CréationQR[i, 10] = B;
                    }
                }
                if ((i + 9) % 2 == 1)
                {
                    if (Info[x + 1] == 1)
                    {
                        CréationQR[i, 9] = B;
                    }
                }
                if ((i + 9) % 2 == 0)
                {
                    if (Info[x + 1] == 0)
                    {
                        CréationQR[i, 9] = B;
                    }
                }
                x += 2;
            }
            for (int i = 13; i > -1; i--)
            {
                if ((i + 10) % 2 == 1)
                {
                    if (Info[x] == 1)
                    {
                        CréationQR[i, 10] = B;
                    }
                }
                if ((i + 10) % 2 == 0)
                {
                    if (Info[x] == 0)
                    {
                        CréationQR[i, 10] = B;
                    }
                }
                if ((i + 9) % 2 == 1)
                {
                    if (Info[x + 1] == 1)
                    {
                        CréationQR[i, 9] = B;
                    }
                }
                if ((i + 9) % 2 == 0)
                {
                    if (Info[x + 1] == 0)
                    {
                        CréationQR[i, 9] = B;
                    }
                }
                x += 2;
            }
            for (int i = 8; i < 12; i++)
            {
                if ((i + 8) % 2 == 1)
                {
                    if (Info[x] == 1)
                    {
                        CréationQR[i, 8] = B;
                    }
                }
                if ((i + 8) % 2 == 0)
                {
                    if (Info[x] == 0)
                    {
                        CréationQR[i, 8] = B;
                    }
                }
                if ((i + 7) % 2 == 1)
                {
                    if (Info[x + 1] == 1)
                    {
                        CréationQR[i, 7] = B;
                    }
                }
                if ((i + 7) % 2 == 0)
                {
                    if (Info[x + 1] == 0)
                    {
                        CréationQR[i, 7] = B;
                    }
                }
                x += 2;
            }
            for (int i = 11; i > 7; i--)
            {
                if ((i + 5) % 2 == 1)
                {
                    if (Info[x] == 1)
                    {
                        CréationQR[i, 5] = B;
                    }
                }
                if ((i + 7) % 2 == 0)
                {
                    if (Info[x] == 0)
                    {
                        CréationQR[i, 5] = B;
                    }
                }
                if ((i + 4) % 2 == 1)
                {
                    if (Info[x + 1] == 1)
                    {
                        CréationQR[i, 4] = B;
                    }
                }
                if ((i + 4) % 2 == 0)
                {
                    if (Info[x + 1] == 0)
                    {
                        CréationQR[i, 4] = B;
                    }
                }
                x += 2;
            }
            for (int i = 8; i < 12; i++)
            {
                if ((i + 3) % 2 == 1)
                {
                    if (Info[x] == 1)
                    {
                        CréationQR[i, 3] = B;
                    }
                }
                if ((i + 3) % 2 == 0)
                {
                    if (Info[x] == 0)
                    {
                        CréationQR[i, 3] = B;
                    }
                }
                if ((i + 2) % 2 == 1)
                {
                    if (Info[x + 1] == 1)
                    {
                        CréationQR[i, 2] = B;
                    }
                }
                if ((i + 2) % 2 == 0)
                {
                    if (Info[x + 1] == 0)
                    {
                        CréationQR[i, 2] = B;
                    }
                }
                x += 2;
            }
            for (int i = 11; i > 7; i--)
            {
                if ((i + 1) % 2 == 1)
                {
                    if (Info[x] == 1)
                    {
                        CréationQR[i, 1] = B;
                    }
                }
                if ((i + 1) % 2 == 0)
                {
                    if (Info[x] == 0)
                    {
                        CréationQR[i, 1] = B;
                    }
                }
                if ((i + 0) % 2 == 1)
                {
                    if (Info[x + 1] == 1)
                    {
                        CréationQR[i, 0] = B;
                    }
                }
                if ((i + 0) % 2 == 0)
                {
                    if (Info[x + 1] == 0)
                    {
                        CréationQR[i, 0] = B;
                    }
                }
                x += 2;
            }
            return CréationQR;
        }

        /// <summary>
        /// Méthode qui agrandi l'image pour la rendre lisible
        /// </summary>
        /// <param name="QRCode"> Matrice support à agrandir </param>
        public void AgrandissementCode(Pixel[,] QRCode)
        {
            Pixel[,] GrosQRCode = new Pixel[QRCode.GetLength(0) * 12, QRCode.GetLength(1) * 12];

            for (int x = 0; x < 21; x++)
            {
                for (int y = 0; y < 21; y++)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        for (int j = 0; j < 12; j++)
                        {
                            GrosQRCode[x * 12 + i, y * 12 + j] = QRCode[x, y];
                        }
                    }
                }
            }
            MyImage.AfficherImage(GrosQRCode, "QRcode.bmp");
        }
    }
}
