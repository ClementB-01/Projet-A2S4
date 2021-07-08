using System;
using System.Collections.Generic;
using System.Text;

namespace Td_n_2
{
    class Pixel
    {
        int rouge;
        int vert;
        int bleu;

        /// <summary>
        /// Constructeur de la classe Pixel
        /// </summary>
        /// <param name="pixel"> Tableau d'entier correspondant à la valeur des couleurs en Rouge, Vert & Bleu constitutives du pixel</param>
        public Pixel(int[] pixel)
        {
            this.rouge = pixel[0];
            this.vert = pixel[1];
            this.bleu = pixel[2];
        }

        /// <summary>
        /// Propriété permettant l'accès et la modification de la composante rouge du pixel
        /// </summary>
        public int Rouge
        {
            get { return rouge; }
            set { rouge = value; }
        }

        /// <summary>
        /// Propriété permettant l'accès et la modification de la composante verte du pixel
        /// </summary>
        public int Vert
        {
            get { return vert; }
            set { vert = value; }
        }

        /// <summary>
        /// Propriété permettant l'accès et la modification de la composante bleue du pixel
        /// </summary>
        public int Bleu
        {
            get { return bleu; }
            set { bleu = value; }
        }


    }
}
