using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Diagnostics;

namespace Td_n_2
{
    class MyImage
    {
        string emplacement, type;
        byte[] file;
        int tailleFichier, tailleOffset, largeur, hauteur;
        int bitCouleur;
        Pixel[,] imageRGB;

        /// <summary>
        /// Constructeur de la classe MyImage
        /// </summary>
        /// <param name="emplacement"> Emplacement de l'image source </param>
        public MyImage(string emplacement)
        {
            this.emplacement = emplacement;
            file = File.ReadAllBytes(emplacement);
            byte[] tabtype = new byte[2];
            byte[] tabtaille = new byte[4];
            byte[] taboffset = new byte[4];
            byte[] tablargeur = new byte[4];
            byte[] tabhauteur = new byte[4];
            byte[] tabcouleur = new byte[2];

            for (int i = 0; i < 4; i++)
            {
                tabtaille[i] = file[i + 2];
                tablargeur[i] = file[i + 18];
                tabhauteur[i] = file[i + 22];
                taboffset[i] = file[i + 34]; // a vérifier si il faut le décalage du au header ou taille image !!

                if (i < 2)
                {
                    tabtype[i] = file[i];
                    tabcouleur[i] = file[i + 28];
                }
            }
            this.tailleFichier = Convertir_Endian_Int(tabtaille);
            this.largeur = Convertir_Endian_Int(tablargeur);
            this.tailleOffset = Convertir_Endian_Int(taboffset);
            this.hauteur = Convertir_Endian_Int(tabhauteur);
            this.type = Convert.ToChar(tabtype[0]) + " " + Convert.ToChar(tabtype[1]);
            this.bitCouleur = Convertir_Endian_Int(tabcouleur);

            this.imageRGB = new Pixel[hauteur, largeur];
            int compteur = 54;
            for (int i = 0; i < imageRGB.GetLength(0); i++)
            {
                for (int j = 0; j < imageRGB.GetLength(1); j++)
                {
                    int[] pixel = { file[compteur], file[compteur + 1], file[compteur + 2] };
                    compteur += 3;
                    this.imageRGB[i, j] = new Pixel(pixel);
                }
            }
        }

        /// <summary>
        /// Propriété qui permetl'accès et l'écriture sur la matrice de pixels
        /// </summary>
        public Pixel[,] ImageRGB
        {
            get
            {
                return imageRGB;
            }
            set
            {
                imageRGB = value;
            }
        }

        /// <summary>
        /// Méthode retournant les infos de l'entête de l'image traitée
        /// </summary>
        /// <returns> Retourne une chaine de caractère comportant les infos </returns>
        public string Tostring()
        {
            return "Taille du fichier : " + tailleFichier + " Largeur : " + largeur + " Hauteur : " + hauteur + " Taille Offset : " + tailleOffset + " Type : " + type + " bitCouleur : " + bitCouleur;
        }

        /// <summary>
        /// Méthode qui convertit en entier un tableau de Little Endian
        /// </summary>
        /// <param name="tableau"> Tableau de Little Endian à convertir </param>
        /// <returns> Retourne l'entier équivalent </returns>
        public static int Convertir_Endian_Int(byte[] tableau)
        {
            int resultat = 0;
            for (int i = 0; i < tableau.Length; i++)
            {
                resultat += Convert.ToInt32(tableau[i] * Math.Pow(2, i * 8));
            }
            return resultat;
        }

        /// <summary>
        /// Méthode convertissant en Little Endian une valeur entière
        /// </summary>
        /// <param name="valeur"> Valeur entière à convertir </param>
        /// <returns> Retourne le tableau de Little Endian équivalent </returns>
        public static byte[] Convertir_Int_Endian(int valeur)
        {
            byte[] tableau = new byte[4];
            if (valeur < Math.Pow(2, 32)) // Vérification de la taille à convertir
            {
                for (int i = 0; i < tableau.Length; i++)
                {
                    tableau[tableau.Length - i - 1] = (byte)Math.DivRem(valeur, (int)(Math.Pow(2, (tableau.Length - i - 1) * 8)), out valeur);
                }
                //int fort = Math.DivRem(valeur, (int)(Math.Pow(2, 24)), out int temp);
                //int deux = Math.DivRem(temp, (int)(Math.Pow(2, 16)), out int temp2);
                //int trois = Math.DivRem(temp2, (int)(Math.Pow(2, 8)), out int faible);
            }
            else
            {
                Console.WriteLine("Conversion impossible valeur trop grande");
            }
            return tableau;
        }

        /// <summary>
        /// Méthode qui permet de créer une nouvelle image
        /// </summary>
        /// <param name="imageAffich"> Matrice de pixel représentant le contenu de l'image </param>
        /// <param name="fichier"> Emplacement de l'image initale </param>
        public static void AfficherImage(Pixel[,] image, string fichier)
        {
            int nHauteur = image.GetLength(0);
            int nLargeur = image.GetLength(1);
            int compteur = 0;

            int newTaille = nHauteur * nLargeur * 3 + 54;
            byte[] ImageInit = File.ReadAllBytes("source.bmp");
            byte[] fileImage = new byte[newTaille];
            for (int i = 0; i < 54; i++)
            {
                fileImage[i] = ImageInit[i];
            }
            for (int i = 2; i < 6; i++)
            {
                ImageInit[i] = Convertir_Int_Endian(newTaille)[compteur];
                compteur++;
            }
            compteur = 0;
            for (int k = 18; k < 22; k++) // Largeur
            {
                fileImage[k] = Convertir_Int_Endian(nLargeur)[compteur];
                compteur++;
            }
            compteur = 0;
            for (int k = 22; k < 26; k++) // Hauteur
            {
                fileImage[k] = Convertir_Int_Endian(nHauteur)[compteur];
                compteur++;
            }
            compteur = 54;
            for (int i = 0; i < nHauteur; i++)
            {
                for (int j = 0; j < nLargeur; j++)
                {
                    fileImage[compteur] = (byte)image[i, j].Rouge;
                    fileImage[compteur + 1] = (byte)image[i, j].Vert;
                    fileImage[compteur + 2] = (byte)image[i, j].Bleu;
                    compteur += 3;
                }
            }

            File.WriteAllBytes(fichier, fileImage);
            //Process.Start("ImageDeSortie.bmp");
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo()
            {
                CreateNoWindow = true,
                Verb = "print",
                FileName = "C:\\Program Files (x86)\\XnView\\xnview.exe",
                Arguments = fichier
            };
            p.Start();
        }

        /// <summary>
        /// Méthode qui convertit un tableau de byte en string
        /// </summary>
        /// <param name="tab"> Tableau à convertir </param>
        /// <returns> Chaîne de caractère équivalente </returns>
        public static string ByteString(byte[] tab)
        {
            string answer = "";
            for (int i = 0; i < tab.Length; i++)
            {
                answer += Convert.ToString(tab[i]);
            }
            return answer;
        }

        /// <summary>
        /// Méthode qui affiche dans la console la matrice des pixels
        /// </summary>
        public void AfficherMatriceRGB(Pixel[,] matrice)
        {
            string affichage = "";
            bool[] table = NonNullNonVide(matrice);
            if (!table[0] && !table[1])
            {
                int dim0 = matrice.GetLength(0);
                int dim1 = matrice.GetLength(1);
                for (int i = 0; i < dim0; i++)
                {
                    affichage = "";
                    for (int j = 0; j < dim1; j++)
                    {
                        //if (imageRGB[i, j] < 10)
                        //{
                        //    affichage += imageRGB[i, j] + "  ";
                        //}
                        //else
                        //{
                        affichage += matrice[i, j].Rouge + " " + matrice[i, j].Vert + " " + matrice[i, j].Bleu + " ";
                        //}
                    }
                    Console.WriteLine(affichage);
                }
            }
        }

        /// <summary>
        /// Méthode qui opère les vérifications sur la matrice avant son affichage
        /// </summary>
        /// <param name="matrice2D"> Matrice à vérifier </param>
        /// <returns> Possibilité d'afficher ou non et raisons </returns>
        public bool[] NonNullNonVide(Pixel[,] matrice2D)
        {
            bool nul = false;
            bool vide = false;
            if (matrice2D == null)
            {
                nul = true;
                Console.WriteLine("La matrice n'a pas d'allocation mémoire");
            }
            else if (matrice2D.GetLength(0) * matrice2D.GetLength(1) == 0)
            {
                vide = true;
                Console.WriteLine("La matrice est vide");
            }
            bool[] table = new bool[2];
            table[0] = nul;
            table[1] = vide;
            return table;
        }

        /// <summary>
        /// Méthode qui opère le traitement en niveau de gris ou noir et blanc de l'image
        /// </summary>
        /// <param name="tmode"> Choix de la conversion et des paramètres de transformation </param>
        /// <param name="limite"> Seuil pour le noir et blanc, par défaut initialiser à la moitié de la plage (128) </param>
        public void NoirBlancGris(int[] tmode, int limite = 128)
        {
            int valeur = 0;
            for (int i = 0; i < imageRGB.GetLength(0); i++)
            {
                for (int j = 0; j < imageRGB.GetLength(1); j++)
                {
                    if (tmode[1] == 0) //Version clarté -> Moyenne des extremums
                    {
                        valeur = (MaxMin(imageRGB[i, j].Rouge, imageRGB[i, j].Vert, imageRGB[i, j].Bleu, 0) + MaxMin(imageRGB[i, j].Rouge, imageRGB[i, j].Vert, imageRGB[i, j].Bleu, 1)) / 2;

                    }
                    else if (tmode[1] == 1) //Version moyenne -> Moyenne des pixels
                    {
                        valeur = (imageRGB[i, j].Rouge + imageRGB[i, j].Vert + imageRGB[i, j].Bleu) / 3;
                    }
                    else if (tmode[2] == 2) //Version luminosité -> Calcul sur R
                    {
                        valeur = (int)(0.21 * imageRGB[i, j].Rouge + 0.587 * imageRGB[i, j].Vert + 0.114 * imageRGB[i, j].Bleu);
                    }
                    else
                    {
                        valeur = (imageRGB[i, j].Rouge + imageRGB[i, j].Vert + imageRGB[i, j].Bleu) / 3;
                    }
                    if (tmode[0] == 0) //Passage en NB
                    {
                        if (valeur > limite) // Décision sur le passage à en blanc (255) ou noir (0)
                        {
                            valeur = 255;
                        }
                        else
                        {
                            valeur = 0;
                        }
                    }
                    // Application
                    int[] pixel = { valeur, valeur, valeur };
                    this.imageRGB[i, j] = new Pixel(pixel);
                }
            }
            //From_Image_To_File(emplacement + "NBG", imageRGB, emplacement); // Création de la nouvelle image
            AfficherImage(imageRGB, "NGB.bmp");
        }

        /// <summary>
        /// Méthode qui crée le négatif d'une image donnée
        /// </summary>
        public void Negatif()
        {
            for (int i = 0; i < imageRGB.GetLength(0); i++)
            {
                for (int j = 0; j < imageRGB.GetLength(1); j++)
                {
                    int rouge = 255 - imageRGB[i, j].Rouge;
                    int vert = 255 - imageRGB[i, j].Vert;
                    int bleu = 255 - imageRGB[i, j].Bleu;
                    int[] pixel = { rouge, vert, bleu };
                    imageRGB[i, j] = new Pixel(pixel);
                }
            }
            AfficherImage(ImageRGB, "Negatif" + emplacement);
        }

        /// <summary>
        /// Méthode qui augmente ou diminue la luminosité d'une image en fonction du pourcentage entré
        /// </summary>
        /// <param name="pourcentage"> Pourcentage de changement souhaité </param>
        public void Luminosité(double pourcentage)
        {
            pourcentage = pourcentage / 255 + 1;
            for (int i = 0; i < imageRGB.GetLength(0); i++)
            {
                for (int j = 0; j < imageRGB.GetLength(1); j++)
                {
                    int rouge, vert, bleu;
                    
                    rouge = Convert.ToInt32(imageRGB[i, j].Rouge * pourcentage);
                    vert = Convert.ToInt32(imageRGB[i, j].Vert * pourcentage);
                    bleu = Convert.ToInt32(imageRGB[i, j].Bleu * pourcentage);

                    if (rouge > 255)
                    {
                        rouge = 255;
                    }
                    if (vert > 255)
                    {
                        vert = 255;
                    }
                    if (bleu > 255)
                    {
                        bleu = 255;
                    }

                    int[] pixel = { rouge, vert, bleu };
                    imageRGB[i, j] = new Pixel(pixel);
                }
            }
            AfficherImage(imageRGB, "Luminosite" + emplacement);
        }

        /// <summary>
        /// Méthode retournant le min ou le max de trois nombres
        /// </summary>
        /// <param name="un"> Premier nombre</param>
        /// <param name="deux"> Deuxième nombre</param>
        /// <param name="trois"> Troisième nombre</param>
        /// <param name="sens"> Max (sens = 0) ou min (sens = 1)</param>
        /// <returns> Retourne l'extremum </returns>
        public int MaxMin(int un, int deux, int trois, int sens)
        {
            int resultat;
            if (sens == 0) // Calcul du maximum de trois nombres
            {
                resultat = Math.Max(un, deux);
                resultat = Math.Max(resultat, trois);
            }
            else // Calcul du minimum de trois nombres
            {
                resultat = Math.Min(un, deux);
                resultat = Math.Min(resultat, trois);
            }
            return resultat;
        }

        /// <summary>
        /// Méthode qui applique un effet miroir selon l'horizontal ou la verticale
        /// </summary>
        /// <param name="sens"> Sens de l'effet miroir </param>
        public void EffetMiroir(string sens = "vertical")
        {
            for (int i = 0; i < imageRGB.GetLength(0); i++)
            {
                for (int j = 0; j < imageRGB.GetLength(1); j++)
                {
                    if (sens == "vertical")
                    {
                        imageRGB[i, j] = imageRGB[i, imageRGB.GetLength(1) - 1 - j];
                    }
                    else
                    {
                        imageRGB[i, j] = imageRGB[imageRGB.GetLength(0) - 1 - i, j];
                    }
                }
            }
            //From_Image_To_File("EM.bmp", imageRGB, emplacement);
            AfficherImage(imageRGB, "Mirroir.bmp");
        }

        /// <summary>
        /// Méthode qui opère la rotation
        /// </summary>
        /// <param name="angle"> Choix de l'angle </param>
        public void Rotation(int angle)
        {
            /*
            Pixel[,] imageRGB2 = new Pixel[hauteur * 2, largeur * 2];
            int cx = hauteur / 2;
            int cy = largeur / 2;
            double angle_cos = Math.Cos(angle / 180 * Math.PI);
            double angle_sin = Math.Sin(angle / 180 * Math.PI);

            for (int i = -cx; i < hauteur * 2; i++)
            {
                for (int j = -cy; j < largeur * 2; j++)
                {
                    int distx = i - cx;
                    int disty = j - cy;
                    int resx = (int)(cx + distx * angle_cos - disty * angle_sin);
                    int resy = (int)(cy + distx * angle_sin - disty * angle_cos);

                    if ((resx >= 0 && resx < hauteur) && (resy >= 0 && resy < largeur))
                    {
                        imageRGB2[i + cx - 1, j + cy - 1] = imageRGB[resx, resy];
                    }
                }
            }
            for (int i = 0; i < hauteur * 2; i++)
            {
                for (int j = 0; j < largeur * 2; j++)
                {
                    if (imageRGB2[i, j] == null)
                    {
                        int[] temp = { 255, 255, 255 };
                        imageRGB2[i, j] = new Pixel(temp);
                    }
                }
            } // Suppression des valeurs null

            AfficherImage(imageRGB2, emplacement);
            //AfficherMatriceRGB(imageRGB2);
            */

            Pixel[,] imageRot = null;
            if (angle == 90)
            {
                imageRot = new Pixel[largeur, hauteur];
                for (int i = 0; i < imageRGB.GetLength(1); i++)
                {
                    for (int j = 0; j < imageRGB.GetLength(0); j++)
                    {
                        imageRot[i, j] = imageRGB[j, largeur - 1 - i];
                    }
                }
            }
            if (angle == 180)
            {
                imageRot = new Pixel[hauteur, largeur];
                for (int i = 0; i < imageRGB.GetLength(0); i++)
                {
                    for (int j = 0; j < imageRGB.GetLength(1); j++)
                    {
                        imageRot[i, j] = imageRGB[imageRGB.GetLength(0) - 1 - i, imageRGB.GetLength(1) - 1 - j];
                    }
                }
            }
            if (angle == 270)
            {
                imageRot = new Pixel[largeur, hauteur];
                for (int i = 0; i < imageRGB.GetLength(1); i++)
                {
                    for (int j = 0; j < imageRGB.GetLength(0); j++)
                    {
                        imageRot[i, j] = imageRGB[hauteur - 1 - j, i];
                    }
                }
            }
            AfficherImage(imageRot, "Rotation.bmp");
        }

        /// <summary>
        /// Diminution ou agrandissement de l'image
        /// </summary>
        /// <param name="sens"> Sens du changement de dimension </param>
        public void Dimension(int sens)
        {
            Pixel[,] imageDim = null;
            if (sens == 0)  //aggrandissement
            {
                imageDim = new Pixel[hauteur * 2, largeur * 2];
                for (int i = 0; i < imageRGB.GetLength(0); i++)
                {
                    for (int j = 0; j < imageRGB.GetLength(1); j++)
                    {
                        imageDim[i * 2, j * 2] = imageRGB[i, j];
                        imageDim[1 + i * 2, j * 2] = imageRGB[i, j];
                        imageDim[i * 2, 1 + j * 2] = imageRGB[i, j];
                        imageDim[1 + i * 2, 1 + j * 2] = imageRGB[i, j];
                    }
                }
            }
            if (sens == 1) //reduction
            {
                imageDim = new Pixel[hauteur / 2, largeur / 2];
                for (int i = 0; i < imageDim.GetLength(0); i++)
                {
                    for (int j = 0; j < imageDim.GetLength(1); j++)
                    {
                        imageDim[i, j] = imageRGB[i * 2, j * 2];
                    }
                }
            }

            AfficherImage(imageDim, "Dimension.bmp");
        }

        /// <summary>
        /// Méthode qui permet de calculer le kernel d'une matrice
        /// </summary>
        /// <param name="matriceK1"> Matrice principale </param>
        /// <param name="matriceK2"> Matrice Kernel </param>
        public void Convolution(int mode, int diviseur = 1, int decalage = 0)
        {
            Pixel[,] matriceK1Mirroir = new Pixel[imageRGB.GetLength(0), imageRGB.GetLength(1)]; //= matriceK1;
            for (int i = 0; i < imageRGB.GetLength(0); i++) //Fige une nouvelle matrice le temps des calculs
            {
                for (int j = 0; j < imageRGB.GetLength(1); j++)
                {
                    matriceK1Mirroir[i, j] = imageRGB[i, j];
                }
            }

            //Créer les matrices de filtrage
            int[,] matriceK2 = { { 0, 0, 0 }, { 0, 1, 0 }, { 0, 1, 0 } }; //Filtre neutre avec diviseur = 1 et decalage = 0

            if (mode == 1) // Filtre flou diviseur = 9 //////////////////// FONCTIONNEL
            {
                matriceK2 = new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            }
            if (mode == 2) // Repoussage §§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§ MOYEN
            {
                matriceK2 = new int[,] { { -2, -1, 0 }, { -1, 0, 1 }, { 0, 1, 2 } };
            }
            if (mode == 3) //Renforcement de contours diviseur=5 | décalage = 1 ////////////////////////////////// FONCTIONNEL
            {
                //matriceK2 = new int[,] { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } };
                matriceK2 = new int[,] { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } };
            }
            if (mode == 4) //Détection de contours diviseur=1 | décalage = 128 + NB ////////////// FONCTIONNEL
            {
                matriceK2 = new int[,] { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } };
            }
            if (mode == 5) //Réhausseur de contraste
            {
                matriceK2 = new int[,] { { -1, -1, -1 }, { -1, 9, -1 }, { -1, -1, -1 } };
            }
            //if (mode == 4) // Filtre de Sobel en X
            //{
            //    matriceK2 = new int[,] { { -1, 0, 1 }, { -2, -0, 2 }, { -1, 0, 1 } };
            //}
            //if (mode == 5) // Filtre de Sobel en Y
            //{
            //    matriceK2 = new int[,] { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };
            //}

            for (int i = 1; i < imageRGB.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < imageRGB.GetLength(1) - 1; j++)
                {
                    imageRGB[i, j] = ResKern(matriceK1Mirroir, matriceK2, i, j, diviseur, decalage);
                }
            }

            AfficherImage(imageRGB, "Convo" + emplacement);
            //return imageRGB;
        }

        /// <summary>
        /// Méthode qui calcule le kernel pour chaque cellule
        /// </summary>
        /// <param name="matriceK1Mirroir"> MatriceK1 figée qui sert de base de calcul </param>
        /// <param name="matriceK2"> Matrice Kernel </param>
        /// <param name="posX"> Position suivant X au moment de l'appel </param>
        /// <param name="posY"> Position suivant Y au moment de l'appel </param>
        /// <returns></returns>
        public Pixel ResKern(Pixel[,] matriceK1Mirroir, int[,] matriceK2, int posX, int posY, int diviseur, int decalage)
        {
            int[] resultat = { 0, 0, 0 };

            int k = 0;
            int l = 0;

            for (int x = posX - 1; x < posX + 2; x++)
            {
                l = 0;
                for (int y = posY - 1; y < posY + 2; y++)
                {
                    resultat[0] += Convert.ToInt32(matriceK1Mirroir[(x), (y)].Rouge * matriceK2[k, l]);
                    resultat[1] += Convert.ToInt32(matriceK1Mirroir[(x), (y)].Vert * matriceK2[k, l]);
                    resultat[2] += Convert.ToInt32(matriceK1Mirroir[(x), (y)].Bleu * matriceK2[k, l]);
                    l++;
                }
                k++;
            }

            //Application du diviseur et du décalage
            for (int d = 0; d < resultat.Length; d++)
            {
                resultat[d] = (resultat[d] + decalage) / diviseur;
            }
            Pixel convo = new Pixel(resultat);

            return convo;
        }

        /// <summary>
        /// Méthode qui fait l'histogramme des couleurs qui compose l'image
        /// </summary>
        public void Histogramme()
        {
            int[] rouge = new int[256];
            int[] vert = new int[256];
            int[] bleu = new int[256];

            for (int i = 0; i < imageRGB.GetLength(0); i++)
            {
                for (int j = 0; j < imageRGB.GetLength(1); j++)
                {
                    rouge[imageRGB[i, j].Rouge]++;
                    vert[imageRGB[i, j].Vert]++;
                    bleu[imageRGB[i, j].Bleu]++;

                }
            }

            int maxBleu = 0;
            int maxVert = 0;
            int maxRouge = 0;
            for (int i = 0; i < rouge.Length; i++) //Recherche de la valeur maximale pour dimendsionner la matrice
            {
                if (maxRouge < rouge[i]) { maxRouge = rouge[i]; }
                if (maxVert < vert[i]) { maxVert = vert[i]; }
                if (maxBleu < vert[i]) { maxBleu = vert[i]; }
            }
            int max = MaxMin(maxRouge, maxVert, maxBleu, 0);

            Pixel[,] histogramme = new Pixel[rouge.Length + vert.Length + bleu.Length, max]; //Création de la matrice de pixel pour l'image histogramme

            int[] pRouge = { 255, 0, 0 }; //Création des pixels de remplissage
            int[] pVert = { 0, 255, 0 };
            int[] pBleu = { 0, 0, 255 };
            int[] pBlanc = { 255, 255, 255 };

            for (int i = 0; i < histogramme.GetLength(0); i++)
            {
                for (int j = 0; j < histogramme.GetLength(1); j++)
                {
                    if (i <= 255)
                    {
                        if (j <= rouge[i])
                        {
                            histogramme[i, j] = new Pixel(pRouge);
                        }
                        else
                        {
                            histogramme[i, j] = new Pixel(pBlanc);
                        }
                    }
                    if (i > 255 && i < 512)
                    {
                        if (j <= vert[i - 256])
                        {
                            histogramme[i, j] = new Pixel(pVert);
                        }
                        else
                        {
                            histogramme[i, j] = new Pixel(pBlanc);
                        }
                    }
                    else if (i >= 512)
                    {
                        if (j <= bleu[i - 512])
                        {
                            histogramme[i, j] = new Pixel(pBleu);
                        }
                        else
                        {
                            histogramme[i, j] = new Pixel(pBlanc);
                        }
                    }
                }
            }

            AfficherImage(histogramme, "Histogramme" + emplacement);
        }

        /// <summary>
        /// Méthode qui cache une image dans une autre
        /// </summary>
        /// <param name="dissimulee"> Image à dissimuler </param>
        public void SteganoEncod(MyImage dissimulee)
        {
            if (dissimulee.imageRGB.GetLength(0) > imageRGB.GetLength(0) || dissimulee.imageRGB.GetLength(1) > imageRGB.GetLength(1))
            {
                Console.WriteLine("Dissimulation impossible car l'image à dissimuler est plus grande que l'image support");
            }
            else
            {
                for (int i = 0; i < dissimulee.imageRGB.GetLength(0); i++) //Permet de mettre à zéro le bit de poids faible
                {
                    for (int j = 0; j < dissimulee.imageRGB.GetLength(1); j++)
                    {
                        int[] tempR1 = Convert_Int_Bit(imageRGB[i, j].Rouge); //Définie l'équivalent binaire du pixel considéré sur l'image support
                        int[] tempV1 = Convert_Int_Bit(imageRGB[i, j].Vert);
                        int[] tempB1 = Convert_Int_Bit(imageRGB[i, j].Bleu);

                        int[] tempR2 = Convert_Int_Bit(dissimulee.imageRGB[i, j].Rouge); // Définie l'équivalent binaire du pixel considéré sur l'image à dissimuler
                        int[] tempV2 = Convert_Int_Bit(dissimulee.imageRGB[i, j].Vert);
                        int[] tempB2 = Convert_Int_Bit(dissimulee.imageRGB[i, j].Bleu);

                        for (int k = 0; k < tempR1.Length; k++) // On tronque les 4 bits faibles de l'image support qu'on remplace par les 4 bits forts de l'image à dissimuler
                        {
                            if (k > 3)
                            {
                                tempR1[k] = tempR2[k - 4];
                                tempV1[k] = tempV2[k - 4];
                                tempB1[k] = tempB2[k - 4];
                            }
                        }

                        int R1 = Convert_Bit_Int(tempR1); //Conversion en entier des nouveaux pixels
                        int V1 = Convert_Bit_Int(tempV1);
                        int B1 = Convert_Bit_Int(tempB1);

                        int[] pixel = { R1, V1, B1 }; // Création du nouveau pixel
                        Pixel nPixel = new Pixel(pixel);

                        imageRGB[i, j] = nPixel; // Insertion du nouveau pixel dans la matrice des pixels
                    }
                }
                AfficherImage(imageRGB, "Stegano" + emplacement);
            }
        }

        /// <summary>
        /// Méthode qui assure le décodage de l'image cachée
        /// </summary>
        public void SteganoDecod()
        {
            for (int i = 0; i < imageRGB.GetLength(0); i++) //Permet de mettre à zéro le bit de poids faible
            {
                for (int j = 0; j < imageRGB.GetLength(1); j++)
                {
                    int[] tempR2 = Convert_Int_Bit(imageRGB[i, j].Rouge); // Définie l'équivalent binaire du pixel considéré sur l'image à dissimuler
                    int[] tempV2 = Convert_Int_Bit(imageRGB[i, j].Vert);
                    int[] tempB2 = Convert_Int_Bit(imageRGB[i, j].Bleu);

                    foreach (int elements in tempR2)
                    {
                        if (elements < 4) // Récupération des bites forts qui étaient en position faible
                        {
                            tempR2[elements] = tempR2[elements + 4];
                            tempV2[elements] = tempV2[elements + 4];
                            tempB2[elements] = tempB2[elements + 4];
                        }/*
                        else //Réinitialisation des bites faibles à 0
                        {
                            tempR2[elements] = 0;
                            tempV2[elements] = 0;
                            tempB2[elements] = 0;
                        }*/
                    }

                    int R2 = Convert_Bit_Int(tempR2); //Conversion en entier des pixels
                    int V2 = Convert_Bit_Int(tempV2);
                    int B2 = Convert_Bit_Int(tempB2);

                    int[] pixel = { R2, V2, B2 }; // Création du nouveau pixel
                    Pixel nPixel = new Pixel(pixel);

                    imageRGB[i, j] = nPixel; //Insertion des pixels
                }
            }
            AfficherImage(imageRGB, "resultat.bmp");
        }

        /// <summary>
        /// Dessine une fractale de Mandelbrot
        /// </summary>
        public void FractaleMandelbrot()
        {
            // Définition de la zone sur laquelle la fractale de Mandelbrot est définie
            double x1 = -2.1;
            double x2 = 0.6;

            double y1 = -1.2;
            double y2 = 1.2;

            int zoom = 1000;
            int iterationMax = 50;

            //Calcul de la taille de l'image
            int largeur = (int)((x2 - x1) * (double)zoom);
            int hauteur = (int)((y2 - y1) * zoom);
            Pixel[,] Mandelbrot = new Pixel[hauteur, largeur]; // On crée la matrice associée

            //Construction graphique
            for (int i = 0; i < Mandelbrot.GetLength(1); i++)
            {
                for (int j = 0; j < Mandelbrot.GetLength(0); j++)
                {
                    double cr = (double)i / zoom + x1; //Coordonnées du point c
                    double ci = (double)j / zoom + y1;

                    double zr = 0; //Termes initiaux de la suite
                    double zi = 0;

                    int compteur = 0;

                    do
                    {
                        double temp = zr;
                        zr = zr * zr - zi * zi + cr;
                        zi = 2 * zi * temp + ci;
                        compteur++;
                    } while (zr * zr + zi * zi < 4 && compteur < iterationMax); //La première condition est une vérification de la divergence ou non du terme de la suite

                    if (compteur == iterationMax) // Cas de la convergence
                    {
                        int[] Pixelnoir = { 50, 50, 50 }; //gris
                        Mandelbrot[j, i] = new Pixel(Pixelnoir);
                    }
                    else // Cas de la divergence
                    {
                        int[] Pixelcouleur = { 0, (255 * compteur) / iterationMax, 0 }; //couleur
                        Mandelbrot[j, i] = new Pixel(Pixelcouleur);
                    }
                }
            }
            AfficherImage(Mandelbrot, "FractaleMandelbrot.bmp");
        }

        /// <summary>
        /// Méthode qui convertie en un tableau d'int représentatif de sa valeur binaire
        /// </summary>
        /// <param name="valeur"> Entier à convertir </param>
        /// <returns> Tableau d'entier </returns>
        public int[] Convert_Int_Bit(int valeur)
        {
            int[] tableau = new int[8];
            if (valeur < 256) // Vérification de la taille à convertir
            {
                for (int i = 0; i < tableau.Length; i++)
                {
                    tableau[i] = Math.DivRem(valeur, (int)(Math.Pow(2, (double)(tableau.Length - i - 1))), out valeur);
                }
            }
            else
            {
                Console.WriteLine("Conversion impossible valeur trop grande");
            }
            return tableau;
        }

        /// <summary>
        /// Méthode qui convertie un tableau d'entier représentatif de la valeur binaire d'un nombre en un entier
        /// </summary>
        /// <param name="valeur"> Tableau d'entier à convertir </param>
        /// <returns> Entier représenté par le tableau en entrée </returns>
        public int Convert_Bit_Int(int[] valeur)
        {
            int resultat = 0;
            for (int i = 0; i < valeur.Length; i++)
            {
                resultat += Convert.ToInt32(valeur[valeur.Length - 1 - i] * Math.Pow(2, i));
            }
            return resultat;
        }

        /// <summary>
        /// Deuxième méthode pour décoder une image cachée
        /// </summary>
        public void SteganoDecod2()
        {
            Pixel[,] ImageCachee = new Pixel[hauteur, largeur];
            for (int i = 0; i < imageRGB.GetLength(0); i++)
            {
                for (int j = 0; j < imageRGB.GetLength(1); j++)
                {
                    int Roriginal = Math.DivRem(imageRGB[i, j].Rouge, 16, out int Rcache); // Division euclidienne
                    int Goriginal = Math.DivRem(imageRGB[i, j].Vert, 16, out int Gcache);
                    int Boriginal = Math.DivRem(imageRGB[i, j].Bleu, 16, out int Bcache);

                    Rcache *= 16;
                    Gcache *= 16;
                    Bcache *= 16;
                    int[] newPixel = { Rcache, Gcache, Bcache };
                    ImageCachee[i, j] = new Pixel(newPixel);
                }
            }
            AfficherImage(ImageCachee, "Resultat.bmp");
        }
    }
}
