using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// A set of classes for handling a bookstore:
namespace Bookstore
{
    using System.Collections;

    // Describes a book in the book list:
    public struct Book
    {
        public string Title;    // Title of the book.
        public string Author;   // Author of the book.
        public decimal Price;   // Price of the book.
        public bool Paperback;  // Is it paperback?
        public Book(string title, string author, decimal price, bool paperBack)
        { Title = title; Author = author; Price = price; Paperback = paperBack; }
    }

    // Declare a delegate type for processing a book:
    public delegate void ProcessBookDelegate(Book book);

    // Maintains a book database.
    public class BookDB
    {   // List of all books in the database:
        ArrayList list = new ArrayList(); //為 BookDB 類別建立實體時，先製造一個 list 陣列

        // Add a book to the database:
        public void AddBook(string title, string author, decimal price, bool paperBack)
        {   //對本類呼叫方法 AddBook 加入一本書時，根據傳入參數為 Book 類別建立實體
            list.Add(new Book(title, author, price, paperBack));
        }

        // Call a passed-in delegate on each paperback book to process it: 
        public void ProcessPaperbackBooks(ProcessBookDelegate processBook)
        {
            foreach (Book b in list)
            {// processBook = 被委派之方法
                if (b.Paperback) processBook(b);// Calling the delegate:
            }
        }
    }
}


// Using the Bookstore classes:
namespace BookTestClient
{
    using Bookstore;

    // Class to total and average prices of books:
    class PriceTotaller
    {
        int countBooks = 0;
        decimal priceBooks = 0.0m;

        internal void AddBookToTotal(Book book)
        {  countBooks += 1;  priceBooks += book.Price;   }

        internal decimal AveragePrice()
        {  return priceBooks / countBooks;    }
    }

    // Class to test the book database:
    class TestBookDB
    {
        // Print the title of the book.
        static void PrintTitle(Book b)
        //第 0 個參數佔 30 字元(靠左); 第 1 個參數格式化為 #.##
        { System.Console.WriteLine("{0,-30} = {1:#.##}", b.Title, b.Price); }
        //第 0 個參數佔 30 字元(靠右); 第 1 個參數格式化為 #.##
        //{ System.Console.WriteLine("{0,30} = {1:#.##}", b.Title, b.Price); }
        //http://blog.csdn.net/xrongzhen/article/details/5477075

        // Execution starts here.
        static void Main()
        {
            BookDB bookDB = new BookDB(); //為命名空間 Bookstore 之類別 BookDB 建立實體
            // Initialize the database with some books:
            bookDB.AddBook("The C Programming Language", "Brian W.", 19.95m, true);
            bookDB.AddBook("The Unicode Standard 2.0", "The Unicode Consortium", 39.95m, true);
            System.Console.WriteLine("Paperback Book Titles:");

            // Create a new delegate object associated with the static 
            // method PrintTitle: 指向靜態方法之委派
            bookDB.ProcessPaperbackBooks(PrintTitle); //書籍名稱 (逐本處理)

            // Get the average price of a paperback by using a PriceTotaller object:
            PriceTotaller totaller = new PriceTotaller(); //為類別 PriceTotaller 建立實體(價格加總)

            // Create a new delegate object associated with the nonstatic 
            // method AddBookToTotal on the object totaller: 指向非靜態方法之委派
            bookDB.ProcessPaperbackBooks(totaller.AddBookToTotal); //書籍加總 (逐本處理)

            System.Console.WriteLine("Average Paperback Book Price: ${0:#.##}",
                    totaller.AveragePrice());

            string nop = System.Console.ReadLine();
        }
    }
}
/* Output:
Paperback Book Titles:
   The C Programming Language
   The Unicode Standard 2.0
   Dogbert's Clues for the Clueless
Average Paperback Book Price: $23.97
*/