using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfEscapeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public enum MessageType
    {
        Error,
        Message,
        Warning
    }
    public partial class MainWindow : Window
    {
        Room currentRoom; // will become useful in later versions
        public MainWindow()
        {
            InitializeComponent();

            // define room
            Room room1 = new Room(
                "bedroom",
                "I seem to be in a medium sized bedroom. There is a locker to the left, a nice rug on the floor, and a bed to the right. ");

            // define items
            Item key1 = new Item(
                "small silver key",
                "A small silver key, makes me think of one I had at highschool.",
                true);
            Item key2 = new Item(
                "large key",
                "A large key. Could this be my way out? ",
                true);
            Item locker = new Item(
                "locker",
                "A locker. I wonder what's inside. ",
                false);

            locker.HiddenItem = key2;
            locker.IsLocked = true;
            locker.Key = key1;
            Item bed = new Item(
                "bed",
                "Just a bed. I am not tired right now. ",
                false);
            Item chair = new Item(
               "Chair",
               "A chair, you can sit on it if you're tired ",
               false);
            Item poster = new Item(
                "Poster",
                "A poster, it covers a large part of the wall",
                true);
            bed.HiddenItem = key1;

            // setup bedroom
            room1.Items.Add(new Item(
                "floor mat",
                "A bit ragged floor mat, but still one of the most popular designs. ",
                true));
            room1.Items.Add(bed);
            room1.Items.Add(locker);
            room1.Items.Add(chair);

            // start game
            currentRoom = room1;
            lblMessage.Content = "I am awake, but cannot remember who I am!? Must have been a hell of a party last night... ";
            txtRoomDesc.Text = currentRoom.Description;
            UpdateUI();
        }

        /// <summary>
        /// Update de items in de ListBoxes
        /// </summary>
        private void UpdateUI()
        {
            lstRoomItems.Items.Clear();
            foreach (Item itm in currentRoom.Items)
            {
                lstRoomItems.Items.Add(itm);
            }
        }

        private void LstItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnCheck.IsEnabled = lstRoomItems.SelectedValue != null; // room item selected
            btnPickUp.IsEnabled = lstRoomItems.SelectedValue != null; // room item selected
            btnUseOn.IsEnabled = lstRoomItems.SelectedValue != null && lstMyItems.SelectedValue != null; // room item and picked up item selected
        }

        private void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            // 1. find item to check
            Item roomItem = (Item)lstRoomItems.SelectedItem;

            // 2. is it locked?
            if (roomItem.IsLocked)
            {
                lblMessage.Content = $"{roomItem.Description}It is firmly locked. ";
                return;
            }

            // 3. does it contain a hidden item?
            Item foundItem = roomItem.HiddenItem;
            if (foundItem != null)
            {
                lblMessage.Content = $"Oh, look, I found a {foundItem.Name}.";
                lstMyItems.Items.Add(foundItem);
                roomItem.HiddenItem = null;
                return;
            }

            // 4. just another item; show description
            lblMessage.Content = roomItem.Description;
        }

        private void BtnUseOn_Click(object sender, RoutedEventArgs e)
        {
            // 1. find both items
            Item myItem = (Item)lstMyItems.SelectedItem;
            Item roomItem = (Item)lstRoomItems.SelectedItem;

            // 2. item doesn't fit
            if (roomItem.Key != myItem)
            {
                lblMessage.Content = "That doesn't seem to work. ";
                return;
            }

            // 3. item fits; other item unlocked
            roomItem.IsLocked = false;
            roomItem.Key = null;
            lstMyItems.Items.Remove(myItem);
            lblMessage.Content = $"I just unlocked the {roomItem.Name}!";
        }

        private void BtnPickUp_Click(object sender, RoutedEventArgs e)
        {
            // 1. find selected item
            Item selItem = (Item)lstRoomItems.SelectedItem;

            if (selItem.IsPortable == false)
            {
                lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.Warning);
                return;
            }

            // 2. add item to your items list
            lblMessage.Content = $"I just picked up the {selItem.Name}. ";
            lstMyItems.Items.Add(selItem);
            lstRoomItems.Items.Remove(selItem);
            currentRoom.Items.Remove(selItem);
        }

        private void BtnDrop_Click(object sender, RoutedEventArgs e)
        {
            Item dropItem = (Item)lstMyItems.SelectedItem;

            lblMessage.Content = $"I just dropped {dropItem.Name}";
            lstMyItems.Items.Remove(dropItem);
            lstRoomItems.Items.Add(dropItem);
            currentRoom.Items.Add(dropItem);
        }

        internal static class RandomMessageGenerator
        {
            private static readonly string[] ErrorMessage =
            {
                "Error you did something wrong",
                "Error You did something completely wrong",
                "Error you did something so wrong it broke the system"
            };
            private static readonly string[] MessageMessage =
            {
                "Message in a bottle",
                "Message is a test",
                "Message is fun!"
            };
            private static readonly string[] WarningMessage =
            {
                "Warning you are breaking the system",
                "Warning be careful",
                "Warning watch out"
            };

            public static string GetRandomMessage(MessageType t)
            {
                string[] messages;
                switch (t)
                {
                    case MessageType.Warning:
                        messages = WarningMessage;
                        break;
                    case MessageType.Error:
                        messages = ErrorMessage;
                        break;
                    case MessageType.Message:
                        messages = MessageMessage;
                        break;
                    default:
                        messages = new string[0];
                        break;
                }

                Random random = new Random();
                int index = random.Next(0, messages.Length);
                return messages[index];
            }
        }
    }
}