using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseManager
{
    class Constants
    {
        public static int X_CORDINATE = 106;
        public static int Y_CORDINATE = 15;
        public static int Y_INCREMENT = 36;
        public static int NO_SELECTION = -1;
        public static int GRID_DETAILS_WIDTH = 656;
        public static int GRID_SCROLL_FACTOR = 5;

        public static double ZERO = 0;

        public static string SYMBOL_NEGATIVE = "-";
        public static string SYMBOL_POSITIVE = "+";
        public static string NULL_STRING = "";	
        
        //Prefix MU is for Manage Users
        public static int MU_MIN_LENGTH = 6;
        public static int MU_MAX_LENGTH = 15;
        public static string MU_INCORRECT_LENGTH = "Please Enter Name between 6 - 15 Alphabets Only !";
        public static string MU_INITIAL_MSG = "Please Enter New User's Display Name Here ...";
    }
}
