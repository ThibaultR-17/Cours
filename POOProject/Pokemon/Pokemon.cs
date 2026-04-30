using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace POOProject.Pokemon
{
    internal class Pokemon
    {
        private string? nom;
        private int maxHp;

        private int actualHp;

        private  PokemonType pkType;

        private String crie;

        private bool enVie = true;

        List<Attaque>? attaques;

        public Pokemon(string name, int maxHp, PokemonType type, List<Attaque> atk)  {
            this.nom = name;
            this.maxHp = maxHp;
            this.actualHp = maxHp;
            this.pkType = type;
            this.crie = GenererChaineAleatoire();
            if (atk.Count < 5){
                this.attaques = atk;
            }
            else
            {
                Console.WriteLine("nombre d'attaque incorect, aucune gardé");
            }

        }

        public String Nom
        {
            get { return nom; }
        }

        public int MaxHp
        {
            get { return maxHp; }
        }

        public int ActualHp
        {
            get { return actualHp; }
        }

        public PokemonType PkType
        {
            get { return pkType; }
        }

        public List<Attaque> Attaques
        {
            get { return attaques ?? new List<Attaque>(); }
        }

        public Attaque Utilise(String atk)
        {
            foreach ( Attaque atks in attaques) { 
                if (atks.Nom == atk) {
                    Console.WriteLine(this.nom + " utilise " + atks.Nom);
                    Console.WriteLine(this.crie);
                    return atks;
                }
            }
            return null;
        }

        private void mort()
        {
            this.enVie = false;
        }

        public void PerdHp(int degat)
        {
            if (degat >= actualHp)
            {
                this.mort();
                Console.WriteLine("coup fatal");
            }
            else
            {
                this.actualHp -= degat;
                Console.WriteLine(degat + "pris ," + actualHp + "restant");
            }
        }

        private string GenererChaineAleatoire(int longueur = 10)
        {
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();

            return new string(Enumerable.Repeat(caracteres, longueur)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public bool EstEnVie()
        {
            return this.enVie;
        }
    }
}
