using System;
using System.Collections;
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

namespace POE_Part2
{
    public partial class MainWindow : Window
    {
        ArrayList reply = new ArrayList();
        ArrayList ignore = new ArrayList();

        user_name check_name = new user_name();

        string username = string.Empty;

        int counting = 0;

        AI_Check ai;

        Interest_handler interests;

        Clean_input clean;

        public MainWindow()
        {
            InitializeComponent();

            new respond(reply, ignore);

            ai = new AI_Check(reply, ignore);

            interests = new Interest_handler();

            clean = new Clean_input();

            voice_greeting greet = new voice_greeting();

            greet.greet();
        }

        private void proceed(object sender, RoutedEventArgs e)
        {
            home_grid.Visibility = Visibility.Hidden;

            username_grid.Visibility = Visibility.Visible;
        }

        private void submit_name(object sender, RoutedEventArgs e)
        {

            //remove spaces and check if empty
            if (string.IsNullOrWhiteSpace(usernames_input.Text))
            {
                MessageBox.Show(
                    "Please enter a username before proceeding.",
                    "Username Required",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }
            username =
                check_name.submit_name(usernames_input, chats);

            username_grid.Visibility = Visibility.Hidden;

            chat_grid.Visibility = Visibility.Visible;
        }

        private void send(object sender, RoutedEventArgs e)
        {
            string rawQuestion =
                question.Text.ToString().Trim();

            if (string.IsNullOrWhiteSpace(rawQuestion))
            {
                error_method("CyberKore",
                    "Please enter a question.");

                return;
            }

            string questions =
                clean.RemoveSpecialCharacters(rawQuestion);

            error_method(username, rawQuestion);

            if (questions.Contains("interested"))
            {
                string[] words = questions.Split(' ');

                string interest_message =
                    interests.save_interest(
                        words,
                        ignore,
                        username);

                error_method("CyberKore", interest_message);
            }

            auto_show_interest();

            string response =
                ai.ai_check(questions, username);

            error_method("CyberKore", response);

            question.Clear();
        }

        private void error_method(string name, string message)
        {
            Border messageBorder = new Border
            {
                Margin = new Thickness(0, 2, 0, 2),
                Padding = new Thickness(5, 3, 5, 3),
                CornerRadius = new CornerRadius(5)
            };

            // Different colors for AI and user
            if (name.ToLower().Contains("cyberkore") ||
                name.ToLower().Contains("chat"))
            {
                messageBorder.Background =
                    new SolidColorBrush(Color.FromRgb(0, 120, 215));

                messageBorder.BorderBrush =
                    new SolidColorBrush(Color.FromRgb(0, 90, 170));
            }
            else
            {
                messageBorder.Background =
                    new SolidColorBrush(Color.FromRgb(0, 0, 0));

                messageBorder.BorderBrush =
                    new SolidColorBrush(Color.FromRgb(50, 50, 50));
            }

            messageBorder.BorderThickness = new Thickness(1);

            TextBlock messageText = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(2)
            };

            Brush nameColor =
                (name.ToLower().Contains("cyberkore") ||
                 name.ToLower().Contains("chat"))
                ? Brushes.AntiqueWhite
                : Brushes.SkyBlue;

            Brush messageColor = Brushes.White;

            messageText.Inlines.Add(new Run
            {
                Text = name + ": ",
                Foreground = nameColor,
                FontWeight = FontWeights.Bold
            });

            messageText.Inlines.Add(new Run
            {
                Text = message,
                Foreground = messageColor
            });

            messageBorder.Child = messageText;

            chats.Items.Add(messageBorder);
        }
        private void auto_show_interest()
        {
            if (counting == 3)
            {
                string reminder =
                    interests.reminder(username);

                if (!string.IsNullOrWhiteSpace(reminder))
                {
                    error_method("CyberKore", reminder);
                }

                counting = 0;
            }
            else
            {
                counting += 1;
            }
        }
    }
}