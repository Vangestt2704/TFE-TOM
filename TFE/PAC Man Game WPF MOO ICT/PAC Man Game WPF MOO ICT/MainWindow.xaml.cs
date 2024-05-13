using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading; // importation d'une bibliothèques permettant d'utiliser "DispatcherTimer"
namespace PAC_Man_Game_WPF_MOO_ICT
{


    public partial class MainWindow : Window
    {
        DispatcherTimer tempsDeJeu = new DispatcherTimer(); // création d'un minuteur d'évenements appelé. 

        bool gauche, droite, bas, haut; // 4 fonctions booléenes pour créer les mouvements de mon personnages.
        bool gauchex, droitex, basx, hautx; // 4 fonctions booléenes pour arreter que mon personnage aille dans cette direction la. 

        int vitesse = 8; // pour la vitesse du personnage, modifiable.
        int vitesseFantome = 10; // pour la vitesse des fantômes, modifiable.

        int deplacementMaxFantome = 160; // limite de déplacement pour le fantôme sur la grille, modifiable.
        int compteurPxFantome; // compte le nombre de pixel que le fantôme a déjà parcouru afin de s'assurer qu'il ne dépasse pas la limite.

        int score = 0; // déclaration du score --> initialisation à 0.

        Rect pacmanHitBox; // création de la hitbox de pacman. Utilisable pour les colisions entre les murs, fantômes et pièces.

        public MainWindow()
        {
            InitializeComponent(); // Permet de charger et initialiser tout les éléments du fichier XAML codé avant
            ConfigPacMan(); // appelle ma fonction principal qui me permet de jouer
        }
        private void Touches(object sender, KeyEventArgs e) // Cette fonction est appelé à chaque fois qu'une touche du clavier est pressée. ( "sender", est l'objet qui déclenche l'événement (celui qui a le focus) et "e" contient des infos sur quel touche est pressée ).
        {
            // Quand la touche directionnel gauche est pressé, et que ma variable d'antimouvement est bien réglé sur False alors la condition est complété.
            if (e.Key == Key.Left && gauchex == false)
            {

                droite = haut = bas = false; // Place Toutes les autres directions sur false afin de ne pas avoir d'erreurs.
                droitex = hautx = basx = false; // Place Tout les antimouvements sur false car ce ne concerne pas cette direction ici.
                gauche = true; // Place la variable booléenne de la bonne direction sur true
                pacman.RenderTransform = new RotateTransform(180, pacman.Width / 2, pacman.Height / 2); // Sert à tourner PacMan à gauche. ( 180 car le PacMan est de base avec le visage à droite, et on divise par deux pour être sur que la rotation se fasse au centre et pas sur un des coins ).
            }
            // Quand la touche directionnel droite est pressé, et que ma variable d'antimouvement est bien réglé sur False alors la condition est complété.
            if (e.Key == Key.Right && droitex == false)
            {

                gauchex = hautx = basx = false; // Place Toutes les autres directions sur false afin de ne pas avoir d'erreurs.
                gauche = haut = bas = false; // Place Tout les antimouvements sur false car ce ne concerne pas cette direction ici.
                droite = true; // Place la variable booléenne de la bonne direction sur true
                pacman.RenderTransform = new RotateTransform(0, pacman.Width / 2, pacman.Height / 2); // Sert à tourner PacMan à droite. ( 0 car le PacMan est de base avec le visage à droite, et on divise par deux pour être sur que la rotation se fasse au centre et pas sur un des coins ).
            }
            // Quand la touche directionnel du haut est pressé, et que ma variable d'antimouvement est bien réglé sur False alors la condition est complété.
            if (e.Key == Key.Up && hautx == false)
            {

                droitex = basx = gauchex = false; // Place Toutes les autres directions sur false afin de ne pas avoir d'erreurs.
                droite = bas = gauche = false; // Place Tout les antimouvements sur false car ce ne concerne pas cette direction ici.
                haut = true; // Place la variable booléenne de la bonne direction sur true
                pacman.RenderTransform = new RotateTransform(-90, pacman.Width / 2, pacman.Height / 2); // Sert à tourner PacMan vers le haut. ( -90 car le PacMan est de base avec le visage à droite, et on divise par deux pour être sur que la rotation se fasse au centre et pas sur un des coins ).
            }
            // Quand la touche directionnel du bas est pressé, et que ma variable d'antimouvement est bien réglé sur False alors la condition est complété.
            if (e.Key == Key.Down && basx == false)
            {

                hautx = gauchex = droitex = false; // Place Toutes les autres directions sur false afin de ne pas avoir d'erreurs.
                haut = gauche = droite = false; // Place Tout les antimouvements sur false car ce ne concerne pas cette direction ici.
                bas = true; // Place la variable booléenne de la bonne direction sur true
                pacman.RenderTransform = new RotateTransform(90, pacman.Width / 2, pacman.Height / 2); // Sert à tourner PacMan. ( 90 car le PacMan est de base avec le visage à droite, et on divise par deux pour être sur que la rotation se fasse au centre et pas sur un des coins ).
            }
        }
        private void ConfigPacMan() // la fonction ici de Configuration va se démarrer à la compilation du programme ( private void, car elle ne permet pas d'être modifié apr d'autres parties du code et ne retourne aucune valeur ).
        {

            MonJeu.Focus(); // Place MyCanvas en focus sur le programme. ( Si pas exploité, PacMan ne pourra pas bougé ).

            tempsDeJeu.Tick += Boucle; // Permet de contrôler le rythme et le fonctionemment du jeu.( Si pas exploité, TOUT les personnages ne bougeront pas ).
            tempsDeJeu.Interval = TimeSpan.FromMilliseconds(20); // Fais que le minuteur donne un .Tick toutes les 20 ms permettant une fluidité, pas trop 1 ms car trop fluide pas 100 car trop lent, modifiable. ( SI pas exploité, TOUT les personnages bougeront à une vitesse anormale ).
            tempsDeJeu.Start(); // Permet de démarrer le temps de jeu. ( Si pas utliser, TOUT les personnages ne bougeront pas ).

            compteurPxFantome = deplacementMaxFantome; // permet de placer le compteur à la limite ma



            // Dans ces lignes de code en dessous c'est juste pour importer des images de pacman et des petits fantômes afin d'avoir un meilleur réalisme dans mon jeu.
            ImageBrush pacmanImage = new ImageBrush();
            pacmanImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/pacman.jpg"));
            pacman.Fill = pacmanImage;
            ImageBrush redGhost = new ImageBrush();
            redGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/red.jpg"));
            redGuy.Fill = redGhost;
            ImageBrush orangeGhost = new ImageBrush();
            orangeGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/orange.jpg"));
            orangeGuy.Fill = orangeGhost;
            ImageBrush pinkGhost = new ImageBrush();
            pinkGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/pink.jpg"));
            pinkGuy.Fill = pinkGhost;
        }
        private void Boucle(object sender, EventArgs e) // Cette fonction Boucle va servir à tout les élements importants du jeu. ( Contrôle de tout les mouvements, les collisions et le score )
        {

            txtScore.Content = "Score: " + score; // Afin de mettre à jour le score à chaque fois que le joueur collecte une pièce. 

            // Je commence les déplacements de mon personnages, si un n'est pas exploité le bonhomme tournera mais n'avancera pas.
            if (droite)
            {
                // Si le joueur appuie sur le bouton pour aller à droite, PacMan se tournera vers la droite et aura la vitesse attribué dans les déclarations de variable 
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + vitesse);
            }
            if (gauche)
            {
                // Si le joueur appuie sur le bouton pour aller à droite, PacMan se tournera vers la droite et aura la vitesse attribué dans les déclarations de variable. Je place un moins devant pour inverser le sens d'avancement.
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - vitesse);
            }
            if (haut)
            {
                // Si le joueur appuie sur le bouton pour aller à droite, PacMan se tournera vers la droite et aura la vitesse attribué dans les déclarations de variable. Je place un moins devant pour inverser le sens d'avancement.
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) - vitesse);
            }
            if (bas)
            {
                // Si le joueur appuie sur le bouton pour aller à droite, PacMan se tournera vers la droite et aura la vitesse attribué dans les déclarations de variable 
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) + vitesse);
            }


            // Fin des mouvements


            // Début des limites de mouvement au bord de la carte.

            //ici je vérifie que si le PacMan est en bas et qu'il essaye de descendre encore plus il ne pourra pas. Je prends la limite de ma fenêtre et je laisse une mage de 80px pour être sur.
            if (bas && Canvas.GetTop(pacman) + 80 > Application.Current.MainWindow.Height)
            {
                // SI PacMan descend à la limite introduite au dessus alors ma fonction de non mouvement s'active, ce qui arrête donc le mouvement.
                basx = true;
                bas = false;
            }
            if (haut && Canvas.GetTop(pacman) < 1)
            {
                // SI PacMan descend à la limite introduite au dessus alors ma fonction de non mouvement s'active, ce qui arrête donc le mouvement.
                hautx = true;
                haut = false;
            }
            if (gauche && Canvas.GetLeft(pacman) - 10 < 1)
            {
                // SI PacMan descend à la limite introduite au dessus alors ma fonction de non mouvement s'active, ce qui arrête donc le mouvement.
                gauchex = true;
                gauche = false;
            }
            if (droite && Canvas.GetLeft(pacman) + 70 > Application.Current.MainWindow.Width)
            {
                // SI PacMan descend à la limite introduite au dessus alors ma fonction de non mouvement s'active, ce qui arrête donc le mouvement.
                droitex = true;
                droite = false;
            }


            // Je crée la HitBox de mon PacMan qui sont définies Le coin supérieur gauche avec le GetLeft et le GetTop, et aussi que la largeur et la hauteur avec le Width et le Height.
            pacmanHitBox = new Rect(Canvas.GetLeft(pacman), Canvas.GetTop(pacman), pacman.Width, pacman.Height);

            // la boucle foreach me permet de parcourir tout les éléments crée qui sont de types <Rectangle> avec la fonction MonJeu.Children
            foreach (var entites in MonJeu.Children.OfType<Rectangle>())
            {

                Rect hitBox = new Rect(Canvas.GetLeft(entites), Canvas.GetTop(entites), entites.Width, entites.Height); // Permet de créer les HitBox de toutes les entités du jeu. Car avec ma boucle juste avant je les appelle tous donc la je créer leur HitBox.

                // Création d'un if qui permet de vérifier si PacMan touche un mur. ( (string)x permet de convertir la valeur de Tag qui peut être donnée en type object ).
                if ((string)entites.Tag == "mur")
                {
                    // Vérifie si quand je vais dans un mur par la gacuche la variable qui me permet d'avancer vers la gauche se désactive et va se placé sur la variable de non mouvement
                    if (gauche == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + 10); // Le + 10 me permet d'avoir une petite marge avant qu'il rentre dans le mur afin de pas être trop collé non plus.
                        gauchex = true;
                        gauche = false;
                    }
                    // Vérifie si quand je vais dans un mur par la droite la variable qui me permet d'avancer vers la gauche se désactive et va se placé sur la variable de non mouvement
                    if (droite == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - 10);
                        droitex = true;
                        droite = false;
                    }
                    // Vérifie si quand je vais dans un mur par le bas la variable qui me permet d'avancer vers la gauche se désactive et va se placé sur la variable de non mouvement
                    if (bas == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) - 10);
                        basx = true;
                        bas = false;
                    }
                    // Vérifie si quand je vais dans un mur par le dessus la variable qui me permet d'avancer vers la gauche se désactive et va se placé sur la variable de non mouvement
                    if (haut == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) + 10);
                        hautx = true;
                        haut = false;
                    }
                }
                // Création d'un if qui permet de vérifier si PacMan touche une pièce. ( (string)entites permet de convertir la valeur de Tag qui peut être donnée en type object ).
                if ((string)entites.Tag == "piece")
                {
                    // Création d'une condition que si PacMan touchait une pièce qui était encore visible alors la pièce se cachait et augmentait le score de 1
                    if (pacmanHitBox.IntersectsWith(hitBox) && entites.Visibility == Visibility.Visible)
                    {

                        entites.Visibility = Visibility.Hidden;

                        score++;
                    }
                }
                // Création d'un if qui permet de vérifier si PacMan touche un fantôme. ( (string)entites permet de convertir la valeur de Tag qui peut être donnée en type object ).
                if ((string)entites.Tag == "fantome")
                {
                    // Création d'une condition qui dit que si PacMan touche un fantôme, j'appelle la fonction FIN et affiche donc un message que tu es mort.  
                    if (pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Fin("les fantômes t'ont eu, cliquez pour rejouer");
                    }
                    // Condition qui permet de vérifier si le fantôme orange est présent dans la partie. ( Nom de l'élement -> converti en chaine de caractèrfe (String) -> Vérifie le nom ).
                    if (entites.Name.ToString() == "orangeGuy")
                    {
                        // Si le fantôme orange est présent il va dans la direction inverse car ( - ) Vitesse fantôme.
                        Canvas.SetLeft(entites, Canvas.GetLeft(entites) - vitesseFantome);
                    }
                    else
                    {
                        // Toutes les autres entités de type fantôme part dans la bonne direction car ( + ) vitesse fantôme.
                        Canvas.SetLeft(entites, Canvas.GetLeft(entites) + vitesseFantome);
                    }
                    // Compteur permettant de limiter les mouvements des fantômes décrémenter à chaque itération de la boucle.
                    compteurPxFantome--;

                    // Si le compteur tombe en dessous de 1 c'est à dire que la valeur de la distance maximale a été atteinte. 
                    if (compteurPxFantome < 1)
                    {
                        // Donc on remet le compteur à la valeur maximale pour que la fantôme puisse se redéplacé.
                        compteurPxFantome = deplacementMaxFantome;
                        // Ensuite on inverse sa vitesse pour qu'il reparte dans l'autre sens.
                        vitesseFantome = -vitesseFantome;
                    }

                }
            }
            // Création d'une condition qui nous dit que si le joueur atteint le score de 114, c'est à dire le nombre maximale de pièce, il a gagné.
            if (score == 92)
            {
                // Montre un message en appelant la fonction FIN, il est raconté que le joueur à gagné.
                Fin("Vous avez gagné, Vous avez colectez toutes les pièces ! ");
            }
        }
        private void Fin(string message) // Ce lance quand tu meurs dans le jeu, ou quand la partie est terminée (string message, car elle contient un message à dire).
        {
            tempsDeJeu.Stop(); // arrête le chrono du jeu.
            MessageBox.Show(message, "PacMan"); // Affiche un message quand le jeu est fini, quand le joueur appuie sur OK dans le message, le jeu recommence.


            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location); // Permet de relancer une partie sans devoir recompiler à chaque occasion.
            Application.Current.Shutdown(); // Permet de relancer le jeu sur la même fenêtre et pas d'en relancer une autre et les accumuler.
        }
    }
}