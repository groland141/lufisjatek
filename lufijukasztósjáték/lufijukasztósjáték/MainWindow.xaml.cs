using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace lufijukasztósjáték
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();

        int gyors = 3;
        int ido = 90;
        Random r = new Random();

        List<Rectangle> itemRemover = new List<Rectangle>();

        ImageBrush backgroundImage = new ImageBrush();

        int lufik;
        int i;

        int kihagyottak;

        bool aktiv;

        int pont;

        MediaPlayer player = new MediaPlayer();
        public MainWindow()
        {
            InitializeComponent();

            timer.Tick += GameEngine;
            timer.Interval = TimeSpan.FromMilliseconds(20);

            backgroundImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/cuccok/background-Image.jpg"));
            canvas.Background = backgroundImage;

            Ujrainditas();
        }

        private void GameEngine(object sender, EventArgs e)
        {
            Pontok.Content = "Pontok: " + pont;

            ido -= 10;

            if (ido < 1)
            {
                ImageBrush lufikep = new ImageBrush();

                lufik += 1;
                if (lufik > 5)
                {
                    lufik = 1;
                }

                switch (lufik)
                {
                    case 1:
                        lufikep.ImageSource = new BitmapImage(new Uri("pack://application:,,,/cuccok/balloon1.png"));
                        break;
                    case 2:
                        lufikep.ImageSource = new BitmapImage(new Uri("pack://application:,,,/cuccok/balloon2.png"));
                        break;
                    case 3:
                        lufikep.ImageSource = new BitmapImage(new Uri("pack://application:,,,/cuccok/balloon3.png"));
                        break;
                    case 4:
                        lufikep.ImageSource = new BitmapImage(new Uri("pack://application:,,,/cuccok/balloon4.png"));
                        break;
                    case 5:
                        lufikep.ImageSource = new BitmapImage(new Uri("pack://application:,,,/cuccok/balloon5.png"));
                        break;
                }

                Rectangle ujlufi = new Rectangle
                {
                    Tag = "Lufi",
                    Height = 50,
                    Width = 50,
                    Fill = lufikep

                };

                Canvas.SetLeft(ujlufi, r.Next(50, 400));
                Canvas.SetTop(ujlufi, 600);

                canvas.Children.Add(ujlufi);

                ido = r.Next(90, 150);

            }

            foreach (var x in canvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "Lufi")
                {

                    i = r.Next(-5, 5);

                    Canvas.SetTop(x, Canvas.GetTop(x) - gyors);
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - (i * -1));
                }

                if (Canvas.GetTop(x) < 20)
                {
                    itemRemover.Add(x);

                    kihagyottak += 1;
                }
            }

            foreach (Rectangle y in itemRemover)
            {
                canvas.Children.Remove(y);
            }


            if (kihagyottak > 10)
            {
                aktiv = false;
                timer.Stop();
                MessageBox.Show("Game over!" + Environment.NewLine + "Kattints az ok-ra a játékhoz!");

                Ujrainditas();
            }

            if (pont > 5)
            {
                gyors = 7;
            }


        }

        private void lufijukasztas(object sender, MouseButtonEventArgs e)
        {
            if (aktiv)
            {
                if (e.OriginalSource is Rectangle)
                {
                    Rectangle activeRec = (Rectangle)e.OriginalSource;

                    player.Open(new Uri("../../cuccok/pop_sound.mp3", UriKind.RelativeOrAbsolute));
                    player.Play();

                    canvas.Children.Remove(activeRec);

                    pont += 1;

                }



            }
        }

        private void Jatekinditasa()
        {
            timer.Start();

            kihagyottak = 0;
            pont = 0;
            ido = 90;
            aktiv = true;
            gyors = 3;
        }

        private void Ujrainditas()
        {
            foreach (var x in canvas.Children.OfType<Rectangle>())
            {
                itemRemover.Add(x);
            }

            foreach (Rectangle y in itemRemover)
            {
                canvas.Children.Remove(y);
            }

            itemRemover.Clear();
            Jatekinditasa();
        }
    }
}
