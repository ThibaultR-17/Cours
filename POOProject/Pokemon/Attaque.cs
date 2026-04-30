using System;
using System.Collections.Generic;
using System.Text;

namespace POOProject.Pokemon
{
    internal class Attaque
    {
        private  PokemonType attaqueType;
        private String nom;
        private int degats;

        public Attaque (PokemonType attaqueType, string nom, int degats)
        {
            this.attaqueType = attaqueType;
            this.nom = nom;
            this.degats = degats;
        }

        public PokemonType AttaqueType
        {
            get { return attaqueType; }
        }

        public String Nom
        {
            get { return nom; }
        }

        public int Degats
        {
            get { return degats; }
        }
    }
}
