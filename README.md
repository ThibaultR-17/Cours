# DegradationImage : 
### MVP++ : à partir d'un JSON de la forme suivante : 
{
  NomImage: {
    url:XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXx
  }
}

### note : 
Commentaire d'executions fait par Gemini (j'avais besoin de voir ou l'execution planté et le code d'erreur/ les retours n'étaient pas précis)

## logique initiale : 
Faire un seul flow par image. Limite : ne connais pas les flow en C#

### telechargement :
 var downloadTasks = root.Select(async values =>
 pour un download asyc pour toutes les images, dans une structure qui associe le nom et l'image.
 
 puis WhenAll avec un toList() pour ne pas redownload plus loin et figer la donnée.

 ### Reduction de l'image : 

 utilisation de la structure pour creer un fichier par image dans le meme dossier que celui d'entrée (optimisation : dossier relatifs)
 Creation d'une liste de résolutions différente pour pouvoir faire un foreach et avoir du parrallelisme dessus ( meme structure pour version avec/sans)
 Sauvegarde de l'image originale avant (pas de transformations faites dessus)

Utiliation de ImageSharp :  optimisation : Resize dans le clone (plus simple a écrire). Logique originale : clone, puis resize dans mutate, mais https://docs.sixlabors.com/api/ImageSharp/SixLabors.ImageSharp.Processing.html

puis sauvegarde.


## Branch : 
J'ai trouvé forEachAsync qui fait un flow par thread avec un async (aide du chat de mitral, j'aimais pas le toList au milieu qui casse le flow et peut poser des problèmes de mémoire si beaucoup d'entrées)
https://learn.microsoft.com/fr-fr/dotnet/api/system.threading.tasks.parallel.foreachasync?view=net-9.0
+ https://docs.sixlabors.com/api/ImageSharp/SixLabors.ImageSharp.Processing.ResizeOptions.html
pour le resizeOption (je n'ai pas tout lu, il y a mieux à faire avec les options disponible je pense)

#  ATTENTION 
pour la branch,  var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }; par défaut dans le code

 
