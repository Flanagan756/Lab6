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

namespace Lab6_Queries_with_Join
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NORTHWNDEntities db = new NORTHWNDEntities();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Ex1Button_Click(object sender, RoutedEventArgs e)
        {
       
            var query = from c in db.Categories
                        join p in db.Products on c.CategoryName equals p.Category.CategoryName
                        orderby c.CategoryName
                        select new {c.CategoryName,p.ProductName };

            var result = query.ToList();

            Ex1lbDisplay.ItemsSource = result;
            EX1TblkCount.Text = result.Count.ToString();

        }

        private void Ex2Button_Click(object sender, RoutedEventArgs e)
        {
         
            var query = from p in db.Products
                        orderby p.Category.CategoryName, p.ProductName
                        select new { Category = p.Category.CategoryName, Product = p.ProductName };

            var result = query.ToList();

            Ex2lbDisplay.ItemsSource = result;
            EX2TblkCount.Text = result.Count.ToString();
        }

        private void Ex3Button_Click(object sender, RoutedEventArgs e)
        {
            //return the total number of orders for product 7
            var query1 = (from detail in db.Order_Details
                          where detail.ProductID == 7
                          select detail);

            //return the total value  of orders for product 7
            var query2 = (from detail in db.Order_Details
                          where detail.ProductID == 7
                          select detail.UnitPrice * detail.Quantity);

            int numberOfOrders = query1.Count();
            decimal totalValue = query2.Sum();
            decimal averageValue = query2.Average();

            Ex3txtbx.Text = string.Format(
                "Total number of orders {0}\nValue of Orders {1:C}\nAverage Order Value{2:C}",
                numberOfOrders, totalValue, averageValue);
        }

        private void Ex4Button_Click(object sender, RoutedEventArgs e)
        {
            //retrieves customers with orders >= 20
            var query = from customer in db.Customers
                        where customer.Orders.Count >= 20
                        select new
                        {
                            Name = customer.CompanyName,
                            OrderCount = customer.Orders.Count
                        };
            Ex4lbDisplay.ItemsSource = query.ToList();
        }
        private void Ex5Btn_Click(object sender, RoutedEventArgs e)
        {
            //Customers with less than 3 orders
            var query = from customer in db.Customers
                        where customer.Orders.Count < 3
                        select new
                        {
                            Company = customer.CompanyName,
                            City = customer.City,
                            Region = customer.Region,
                            OrderCount = customer.Orders.Count
                        };
            Ex5lbDisplay.ItemsSource = query.ToList();
        }

        private void Ex6Btn_Click(object sender, RoutedEventArgs e)
        {
            //Selecting a customer retrieves the total amount of orders from that customer 
            //displayed in a textblock to the right.
            var query = from customer in db.Customers
                        orderby customer.CompanyName
                        select customer.CompanyName;

            Ex6lbDisplay.ItemsSource = query.ToList();
        }
 

        private void Ex6lbDisplay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string company = (string)Ex6lbDisplay.SelectedItem;

            if (company != null)
            {
                var query = (from detail in db.Order_Details
                             where detail.Order.Customer.CompanyName == company
                             select detail.UnitPrice * detail.Quantity).Sum();

                EX6TblEx6OrderSummary.Text = string.Format("Total for supplier {0}\n\n{1:C}", company, query);
            }
        }

        private void Ex7Btn_Click(object sender, RoutedEventArgs e)
        {
            var query = from p in db.Products
                        group p by p.Category.CategoryName into g
                        orderby g.Count() descending
                        select new
                        {
                            Category = g.Key,
                            Count = g.Count()
                        };
            Ex7lbDisplay.ItemsSource = query.ToList();
        }
    }
}
