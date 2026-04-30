using POOProject.Pokemon;
String nom;
PokemonType typeChoisi;
int maxHp;

String attaqueNom;
int attaquedegat;
PokemonType attaqueType;

Pokemon poke1;
Pokemon poke2;

Console.WriteLine("Creer ton pokemon!");

Console.WriteLine("Nom :");
 nom = Console.ReadLine();

Console.WriteLine("choisis son type parmis les suivants :");

foreach(PokemonType typedispo in Enum.GetValues<PokemonType>())
{
    Console.WriteLine(typedispo.ToString());
}

string saisie = Console.ReadLine();

if (Enum.TryParse(saisie, true, out typeChoisi))
{
    Console.WriteLine($"Vous avez choisi le type : {typeChoisi}");
}
else
{
    Console.WriteLine("Ce type de Pokémon n'existe pas.");
}

Console.WriteLine("combien a til de PV");
maxHp=Int32.Parse(Console.ReadLine());

Console.WriteLine("il lui faut une attaque!");

Console.WriteLine("nom de l'attaque");
attaqueNom=Console.ReadLine();
Console.WriteLine("degats de l'attaque");
attaquedegat = Int32.Parse(Console.ReadLine());

//pas le temps

attaqueType = typeChoisi;

Console.WriteLine("MATCH MIROIR");
poke1 = new Pokemon(
    nom,
    maxHp,
    typeChoisi,
    new List<Attaque> { new Attaque(attaqueType, attaqueNom, attaquedegat) }
);

poke2 = new Pokemon(
    nom,
    maxHp,
    typeChoisi,
    new List<Attaque> { new Attaque(attaqueType, attaqueNom, attaquedegat) }
);

while (poke1.EstEnVie() && poke2.EstEnVie())
{
    Console.WriteLine("-------------------------------------------------------------------------------------");
    Console.WriteLine("poke1 commence (pas le temps pour l'intiative");
    Console.WriteLine("quelle attaque?");
    foreach(Attaque atk in poke1.Attaques)
    {
        Console.WriteLine(atk.Nom);
    }
    Attaque atk1 = poke1.Utilise(Console.ReadLine());
    if (atk1 != null)
    {
        poke2.PerdHp(atk1.Degats);
    }
    if (poke2.EstEnVie())
    {
        Console.WriteLine("au tour du poke 2 pas desavantagé");
        Console.WriteLine("quelle attaque?");
        foreach (Attaque atk in poke2.Attaques)
        {
            Console.WriteLine(atk.Nom);
        }
        Attaque atk2 = poke1.Utilise(Console.ReadLine());
        if (atk2 != null)
        {
            poke1.PerdHp(atk2.Degats);
        }
    }
    Console.WriteLine("FIN DU TOUR");
    Console.WriteLine("-------------------------------------------------------------------------------------");
}

if (poke1.EstEnVie() && !poke2.EstEnVie())
{
    Console.WriteLine("poke1 gagne");
}else if (!poke1.EstEnVie() && poke2.EstEnVie())
{
    Console.WriteLine("pas possible mais ok");
}else
{
    Console.WriteLine("pas possible mais égalité");
}

