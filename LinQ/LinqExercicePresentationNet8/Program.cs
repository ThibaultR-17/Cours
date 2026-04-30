using DataSources;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

//Console.WriteLine("a rechercher :");
//string recherche = Console.ReadLine();
//int taillePage = 20;
//int numPage = 0;

//var allAlbums = ListAlbumsData.ListAlbums;

var allAlbums = File.ReadLines("C:\\Users\\thibm\\Desktop\\Alternance\\IPI\\cours\\CSharpDotNet\\LinQ\\DataSources\\Text\\Albums.txt")
    .Select(line => line.Split(':'))
    .Select(c => new Album(int.Parse(c[0]), c[1], 0));

foreach (var album in allAlbums)
{
    XElement albums = new XElement("album",
        new XElement("AlbumId", album.AlbumId),
        new XElement("Title", album.Title)

        );
    Console.WriteLine(albums.ToString());
}



//var allArtiste = ListArtistsData.ListArtists;


//var results = from artist in allArtiste
//              join album in allAlbums on artist.ArtistId equals album.ArtistId
//              where album.Title.Contains(recherche, StringComparison.InvariantCultureIgnoreCase)
//              orderby album.ArtistId, album.Title descending
//              group album by artist;




////var results = from row in allAlbums
////              where row.Title.Contains(recherche, StringComparison.InvariantCultureIgnoreCase)
////              group row by row.ArtistId into Groupe
////              orderby Groupe.Key descending
////              select new
////              {
////                  Artiste = Groupe.Key,
////                  album = from album in Groupe orderby album.Title select $"Album n° {album.AlbumId} : {album.Title}"
////              };

////var results = allAlbums.Where(a => a.Title.Contains(recherche, StringComparison.InvariantCultureIgnoreCase)).GroupBy(a => a.ArtistId).Select(a => new 
////{
////    Artiste = a.Key,
////    album = a.OrderBy(a => a.Title).Select(a => $"Album n°{a.AlbumId} : {a.Title}" )
////});

//Console.WriteLine("20 resultats par page");
//bool stop = false;
//while (stop == false)

//{
//    var currentResults = results.Skip(taillePage * numPage).Take(taillePage);
//    foreach (var groupe in currentResults)
//    {
//        Console.WriteLine("Artiste n°" + groupe.Key.Name);
//        foreach (var album in groupe)
//        {
//            Console.WriteLine("       ---->" + album.Title);
//        }
//    }
//    Console.WriteLine("press a to continue, or anything to leave");
//    if (Console.ReadLine() == "a")
//    {
//        numPage++;
//    }
//    else
//    {
//        stop = true;
//    }
//}


