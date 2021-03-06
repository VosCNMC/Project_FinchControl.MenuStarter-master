﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using FinchAPI;

namespace Project_FinchControl
{

    // **************************************************
    //
    // Title: Finch Control - Menu Starter
    // Description: Starter solution with the helper methods,
    //              opening and closing screens, and the menu
    // Application Type: Console
    // Author: Vos, Conner
    // Dated Created: 1/22/2020
    // Last Modified: 11/06/2020
    //
    // **************************************************

    /// <summary>
    /// List of Commands
    /// </summary>
        public enum Command
        {
            NONE,
            MOVEFORWARD,
            MOVEBACKWARD,
            STOPMOTORS,
            WAIT,
            TURNRIGHT,
            TURNLEFT,
            LEDON,
            LEDOFF,
            GETTEMPERATURE,
            HONK,
            DONE
        }
    
    class Program
    {
        /// <summary>
        /// first method run when the app starts up
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            DisplayLoginRegister();
            SetTheme();
            DisplayMenuScreen();
            DisplayClosingScreen();
        }
        /// <summary>
        /// Log-in screen, asks users if they have an account in this program
        /// </summary>
        static void DisplayLoginRegister()
        {
            bool responseValid = false;
            string userResponse;
            DisplayScreenHeader("Login/Register:");
            do
            {
                Console.WriteLine("Have you been registered with this program? ( yes | no)");
                userResponse = Console.ReadLine();
                if (userResponse.ToLower() == "yes")
                {
                    responseValid = true;
                    break;

                }
                else if (userResponse.ToLower() == "no")
                {
                    DisplayRegisterUser();
                    responseValid = true;
                    break;

                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Error!\nPlease reply with yes or no!\n");
                }
            } while (responseValid != true);
            DisplayLogin();
        }
        /// <summary>
        /// User login screen
        /// </summary>
        static void DisplayLogin()
        {
            string userName;
            string password;
            bool validLogin = false ;

            do
            {
                DisplayScreenHeader("Login");

                Console.WriteLine();
                Console.Write("\tPlease enter your user name: ");
                userName = Console.ReadLine();
                Console.Write("\tPlease enter your password: ");
                password = Console.ReadLine();

                validLogin = IsValidLoginInfo(userName , password);

                Console.WriteLine();
                if (validLogin == true)
                {
                    Console.WriteLine("\tYou are now logged in!");
                }
                else 
                {
                    Console.WriteLine("\tSorry, it appears either your password or user name is incorrect!");
                    Console.WriteLine("\tPlease try again!");
                }
                DisplayContinuePrompt();
                
                    Console.Clear();

            } while (validLogin != true);
        }
        /// <summary>
        /// Checks to see if user input is indeed valid
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        static bool IsValidLoginInfo(string userName, string password)
        {
            List<(string userName, string password)> registeredLoginInfo = new List<(string userName, string password)>();
            bool validUser = false;

            registeredLoginInfo = ReadLoginInfoData();

            ///
            /// Loops through list of registered users and checks each one
            ///
            foreach ((string userName, string password) userLoginInfo in registeredLoginInfo)
            {
                if ((userLoginInfo.userName == userName) && (userLoginInfo.password == password))
                {
                    validUser = true;
                    break;
                }
            }

            return validUser;
        }
        /// <summary>
        /// Reads login info data
        /// </summary>
        /// <returns></returns>
        static List<(string userName, string password)> ReadLoginInfoData()
        {
            string dataPath = @"D:\C#\Project_FinchControl.MenuStarter-master\Project_FinchControl\Data\Logins.txt";

            string[] loginInfoArray;
            (string userName, string password) loginInfoTuple;

            List<(string userName, string password)> registeredUserLoginInfo = new List<(string userName, string password)>();
            loginInfoArray = File.ReadAllLines(dataPath);

            //
            // Loop through the array
            // splitting the user name and password into tuple 
            // add tuple to the list
            //
            foreach (string loginInfoText in loginInfoArray)
            {
                loginInfoArray = loginInfoText.Split(',');

                loginInfoTuple.userName = loginInfoArray[0];
                loginInfoTuple.password = loginInfoArray[1];

                registeredUserLoginInfo.Add(loginInfoTuple);
            }

            return registeredUserLoginInfo;
        }

        /// <summary>
        /// If user doesn't have an account, they make one here
        /// </summary>
        private static void DisplayRegisterUser()
        {
            string userName;
            string password;
            bool completed = false;

            DisplayScreenHeader("Registering A New Account");
            while (completed != true) 
            {
                Console.Write("\tPlease enter your chosen user name!: ");
                userName = Console.ReadLine();
                Console.Write("\tPlease enter your chosen password!: ");
                password = Console.ReadLine();
                Console.WriteLine($"\nSo, your chosen name is: {userName}\nChosen password is: {password}");
                Console.WriteLine("Is this information correct? (yes | no)");
                if (Console.ReadLine().ToLower() == "yes")
                {
                    completed = true;
                    WriteLoginInfoData(userName, password);
                    Console.WriteLine("Your log-in information has now been saved.");
                    DisplayContinuePrompt();
                }
                else 
                {
                    Console.Clear();
                }
            }
        }
        /// <summary>
        /// This Method writes down new user info into a text file
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        static void WriteLoginInfoData(string userName, string password)
        {
            string dataPath = @"D:\C#\Project_FinchControl.MenuStarter-master\Project_FinchControl\Data\Logins.txt";
            string loginInfoText;

            loginInfoText = userName + "," + password + "\n";

            File.AppendAllText(dataPath, loginInfoText);
        }

        /// <summary>
        /// setup the console theme
        /// </summary>
        static void SetTheme()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Main Menu                                 *
        /// *****************************************************************
        /// </summary>
        static void DisplayMenuScreen()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;

            Finch finchRobot = new Finch();

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Connect Finch Robot");
                Console.WriteLine("\tb) Talent Show");
                Console.WriteLine("\tc) Data Recorder");
                Console.WriteLine("\td) Alarm System");
                Console.WriteLine("\te) User Programming");
                Console.WriteLine("\tf) Disconnect Finch Robot");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayConnectFinchRobot(finchRobot);
                        break;

                    case "b":
                        TalentShowDisplayMenuScreen(finchRobot);
                        break;

                    case "c":
                        dataRecorderDisplayMenuScreen(finchRobot);
                        break;

                    case "d":
                        LightAlarmDisplayMenuScreen(finchRobot);
                        break;

                    case "e":
                        UserProgrammingDisplayMenuScreen(finchRobot);
                        break;

                    case "f":
                        DisplayDisconnectFinchRobot(finchRobot);
                        break;

                    case "q":
                        DisplayDisconnectFinchRobot(finchRobot);
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }

        static void UserProgrammingDisplayMenuScreen(Finch finchRobot)
        {
            string menChoice;
            bool quitMenu = false;
            //
            // Command Parameter Tuple
            //

            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledBrightness = 0;
            commandParameters.waitSeconds = 0;


            List<Command> commands = new List<Command>();

            do
            {
                DisplayScreenHeader("User Programming Menu");

                //
                // User menu Choice
                //

                Console.WriteLine("\tA) Set Command Parameters");
                Console.WriteLine("\tB) Add Commands");
                Console.WriteLine("\tC) View Commands");
                Console.WriteLine("\tD) Execute Commands");
                Console.WriteLine("\tQ) Quit");
                Console.WriteLine("Enter Choice: ");
                menChoice = Console.ReadLine().ToLower();

                switch (menChoice)
                {
                    case "a":
                        commandParameters = UserProgrammingDisplayGetCommandParameters();
                        break;

                    case "b":
                        UserProgrammingDisplayGetFinchCommands(commands);
                        break;

                    case "c":
                        UserProgrammingDisplayFinchCommands(commands);
                        break;

                    case "d":
                        UserProgrammingDisplayExecuteFinchCommands(finchRobot, commands, commandParameters);
                        break;

                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;







                }

            } while (!quitMenu);
        }
        /// <summary>
        /// Executing User Commands
        /// </summary>
        /// <param name="finchRobot">Finch Robot</param>
        /// <param name="commands"> list of user implemented Commands</param>
        /// <param name="commandParameters"> specifics user requested</param>
        static void UserProgrammingDisplayExecuteFinchCommands(Finch finchRobot, List<Command> commands, (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters)
        {
            // Variables
            int motorSpeed = commandParameters.motorSpeed;
            int ledBrightness = commandParameters.ledBrightness;
            int waitSeconds = (int)(commandParameters.waitSeconds * 1000); // Finch Considers this in milliseconds, so math must be used to convert.
            string commandFeedback = "";
            const int TURNING_MOTOR_SPEED = 100;

            DisplayScreenHeader("Execute Finch Commands");
            Console.WriteLine("The Finch Robot is now ready to execute your list of commands.");

            DisplayContinuePrompt();
            Console.WriteLine();

            // Executing Commands!

            foreach (Command command in commands)
            {
                switch (command) 
                {
                    case Command.NONE:
                        break;

                    case Command.MOVEFORWARD:
                        finchRobot.setMotors(motorSpeed,motorSpeed);
                        commandFeedback = Command.MOVEFORWARD.ToString();
                        break;

                    case Command.MOVEBACKWARD:
                        finchRobot.setMotors(-motorSpeed, -motorSpeed);
                        commandFeedback = Command.MOVEBACKWARD.ToString();
                        break;

                    case Command.STOPMOTORS:
                        finchRobot.setMotors(0, 0);
                        commandFeedback = Command.MOVEBACKWARD.ToString();
                        break;

                    case Command.WAIT:
                        finchRobot.wait(waitSeconds);
                        commandFeedback = Command.WAIT.ToString();
                        break;

                    case Command.TURNRIGHT:
                        finchRobot.setMotors(TURNING_MOTOR_SPEED, -TURNING_MOTOR_SPEED);
                        commandFeedback = Command.TURNRIGHT.ToString();
                        break;

                    case Command.TURNLEFT:
                        finchRobot.setMotors(-TURNING_MOTOR_SPEED, TURNING_MOTOR_SPEED);
                        commandFeedback = Command.TURNRIGHT.ToString();
                        break;

                    case Command.LEDON:
                        finchRobot.setLED(ledBrightness, ledBrightness, ledBrightness);
                        finchRobot.noteOn(600);
                        finchRobot.wait(500);
                        commandFeedback = Command.LEDON.ToString();
                        break;

                    case Command.LEDOFF:
                        finchRobot.setLED(0,0,0);
                        finchRobot.noteOn(300);
                        finchRobot.wait(500);
                        commandFeedback = Command.LEDOFF.ToString();
                        break;

                    case Command.GETTEMPERATURE:
                        commandFeedback = $"Temperature Reading is: {finchRobot.getTemperature().ToString("n2")}";
                        break;

                    case Command.HONK:
                        finchRobot.noteOn(400);
                        finchRobot.wait(2000);
                        commandFeedback = Command.HONK.ToString();
                        break;

                    default:
                        break;

                }
                Console.WriteLine($"\t{commandFeedback}");
            }

            // Commands finished
            DisplayMenuPrompt("User Programming");
        }
        /// <summary>
        /// Displays Commands that were inputed by user
        /// </summary>
        /// <param name="commands"></param>
        static void UserProgrammingDisplayFinchCommands(List<Command> commands)
        {
            DisplayScreenHeader("Finch Robot Commands");

            Console.WriteLine("\tCurrent list of commands you've selected:");
            Console.WriteLine();
            foreach (Command command in commands)
            {
                Console.WriteLine($"\t{command}");
            }

            DisplayMenuPrompt("User Programming");
        }

        /// <summary>
        /// Reads off list of available commands for user
        /// </summary>
        /// <param name="commands"></param>
        static void UserProgrammingDisplayGetFinchCommands(List<Command> commands)
        {
            Command command = Command.NONE;

            DisplayScreenHeader("Finch Robot Commands");

            //
            // List of Commands
            //
            int commandCount = 1;
            Console.WriteLine("\tList of Available Commands");
            Console.WriteLine();
            Console.WriteLine();
            foreach (string commandName in Enum.GetNames(typeof(Command)))
            {
                Console.WriteLine($"{commandName.ToLower()}");
                if (commandCount % 5 == 0);
                commandCount++;
      
            }   
            Console.WriteLine();

            // USER INPUT LOOP
            while (command != Command.DONE)
            {
                Console.WriteLine("\tEnter A Command:");

                if (Enum.TryParse(Console.ReadLine().ToUpper(), out command))
                {
                    commands.Add(command);
                }
                else
                {
                    Console.WriteLine("*********************************************");
                    Console.WriteLine("*Please Enter a command from the list above.*");
                    Console.WriteLine("*********************************************");
                    Console.WriteLine();
                
                }

            }
            DisplayMenuPrompt("User Programming");
        }

        /// <summary>
        /// *************************************
        /// Get Command Parameters from the USER
        /// *************************************
        /// </summary>
        /// <returns>Command Parameters</returns>
        static (int motorSpeed, int ledBrightness, double waitSeconds) UserProgrammingDisplayGetCommandParameters()
        {
            string userResponse = null;
            bool answer = false;
            bool validAnswer = false;
            int numChecker = 0;
            DisplayScreenHeader("Comman Parameters");

            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledBrightness = 0;
            commandParameters.waitSeconds = 0;

            // Checks to see if answer is between (1 - 255)
            // MOTOR SPEED 
            while (!validAnswer) // MOTOR SPEED
            {
                Console.WriteLine("Enter Motor Speed [ 1 - 255]:");
                userResponse = Console.ReadLine();
                answer = int.TryParse(userResponse, out numChecker);
                if (answer = true)
                {
                    if (numChecker > 255 || numChecker < 1)
                    {
                        Console.WriteLine("Error: please enter a valid number!");
                    }
                    else
                    {
                        commandParameters.motorSpeed = numChecker;
                        validAnswer = true;
                        break;
                    }
                }
            }
            validAnswer = false;
            answer = false;
            //LED BRIGHTNESS
            while (!validAnswer) // LED BRIGHTNESS
            {
                Console.WriteLine("Enter LED BRIGHTNESS [ 1 - 255]:");
                userResponse = Console.ReadLine();
                answer = int.TryParse(userResponse, out numChecker);
                if (answer == true)
                {
                    if (numChecker > 255 || numChecker < 1)
                    {
                        Console.WriteLine("Error: please enter a valid number!");
                    }
                    else
                    {
                        commandParameters.ledBrightness = numChecker;
                        validAnswer = true;
                        break;
                    }
                }
                else if (answer == false)
                {
                    Console.WriteLine("Please enter a valid whole number!");
                }
            }
            validAnswer = false;
            answer = false;
            //Wait Seconds
            while (!validAnswer) // Wait Seconds
            {
                Console.WriteLine("How many seconds would you like to wait?:");
                userResponse = Console.ReadLine();
                answer = double.TryParse(userResponse, out commandParameters.waitSeconds);
                if (answer == true)
                {
                    if (numChecker > 255 || numChecker < 1)
                    {
                        Console.WriteLine("Error: please enter a valid number!");
                    }
                    else
                    {
                        validAnswer = true;
                        break;
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine($"Motor Speed has been set to: {commandParameters.motorSpeed} out of 255.");
            Console.WriteLine($"LED brightness has been set to: {commandParameters.ledBrightness} out of 255.");
            Console.WriteLine($"Your wait time has been set to: {commandParameters.waitSeconds} seconds.");
            //Display Menu for user
            DisplayMenuPrompt("User Programming");
            return commandParameters;
        }
        #region TALENT SHOW

        /// <summary>
        /// *****************************************************************
        /// *                     Talent Show Menu                          *
        /// *****************************************************************
        /// </summary>
        static void TalentShowDisplayMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitTalentShowMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Talent Show Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Light and Sound");
                Console.WriteLine("\tb) Dance with me");
                Console.WriteLine("\tc) Sing a Song");
                Console.WriteLine("\td) Mixing it up!");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayLightAndSound(finchRobot);
                        break;

                    case "b":
                        DisplayDanceWithMe(finchRobot);
                        break;

                    case "c":
                        SingASong(finchRobot);
                        break;

                    case "d":
                        MixItUp(finchRobot);
                        break;

                    case "q":
                        quitTalentShowMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;

                }
            } while (!quitTalentShowMenu);

            /// <summary>
            /// *****************************************************************
            /// *               Talent Show > Light and Sound                   *
            /// *****************************************************************
            /// </summary>
            /// <param name="finchRobot">finch robot object</param>
            static void DisplayLightAndSound(Finch finchRobot)
            {
                Console.CursorVisible = false;

                DisplayScreenHeader("Light and Sound");

                Console.WriteLine("\tThe Finch robot will now show off its glowing talent!");
                DisplayContinuePrompt();

                for (int lightSoundLevel = 0; lightSoundLevel < 10; lightSoundLevel++)
                {
                    finchRobot.setLED(lightSoundLevel, lightSoundLevel, lightSoundLevel);
                    finchRobot.noteOn(lightSoundLevel * 100);
                    finchRobot.wait(200);
                    finchRobot.setLED(0, 0, 0);
                    finchRobot.noteOff();
                    finchRobot.wait(100);
                }

                DisplayMenuPrompt("Talent Show Menu");
            }
            // DANCE WITH ME METHOD
            static void DisplayDanceWithMe(Finch finchRobot)
            {
                DisplayScreenHeader("Dance With Me!");

                Console.WriteLine("\tThe Finch robot will do a quick move!");
                for (int i = 0; i < 3; i++)
                {
                    finchRobot.setMotors(255, -255);
                    finchRobot.wait(100);
                    finchRobot.setMotors(0, 0);
                    finchRobot.wait(100);
                    finchRobot.setMotors(-255, 255);
                    finchRobot.wait(100);
                    finchRobot.setMotors(0, 0);
                    finchRobot.wait(100);
                }


                DisplayContinuePrompt();
            }

            ///MIX IT UP METHOD
            static void MixItUp(Finch finchRobot)
            {
                DisplayScreenHeader("Mix it up!");


                Console.WriteLine("\tThe Finch robot will glow and dance!");
                for (int i = 0; i < 3; i++)
                {
                    finchRobot.setMotors(255, -255);
                    finchRobot.wait(100);
                    finchRobot.setLED(i * 200, i * 100, 0);
                    finchRobot.setMotors(0, 0);
                    finchRobot.wait(100);
                    finchRobot.setLED(200, i * 40, i * 15);
                    finchRobot.setMotors(-255, 255);
                    finchRobot.wait(100);
                    finchRobot.setMotors(0, 0);
                    finchRobot.wait(100);
                    finchRobot.setLED(0, 0, 0);
                }


                DisplayContinuePrompt();

            }
            //SING A SONG METHOD
            static void SingASong(Finch finchRobot)
            {
                Console.CursorVisible = false;

                DisplayScreenHeader("Sing a Song!");

                Console.WriteLine("\tThe Finch robot will now sing a small tune!");
                DisplayContinuePrompt();

                finchRobot.noteOn(1600);
                finchRobot.wait(1000);
                finchRobot.noteOff();
                finchRobot.noteOn(800);
                finchRobot.wait(1000);
                finchRobot.noteOff();
                finchRobot.noteOn(650);
                finchRobot.wait(1000);
                finchRobot.noteOff();
                finchRobot.noteOn(1050);
                finchRobot.wait(1000);
                finchRobot.noteOff();
                finchRobot.wait(1000);


                DisplayMenuPrompt("Talent Show Menu");
            }
        }
        #endregion

        #region Alarm System
        /// <summary>
        /// ALARM SYSTEM MENU
        /// </summary>
        /// <param name="finchRobot"></param>
        static void LightAlarmDisplayMenuScreen(Finch finchRobot)
        { 
            bool quitMenu = false;
            string menuChoice = "";
            // Variables
            string sensorsToMonitor = "";
            string rangeType = "";
            int minMaxValue = 0;
            int timeToMonitor = 0;


            do
            {
                DisplayScreenHeader("Alarm System Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Sensors To Monitor");
                Console.WriteLine("\tb) Set Range Type");
                Console.WriteLine("\tc) Set Minimum/Maximum Threshold Value"); 
                Console.WriteLine("\td) Set Time to Monitor");
                Console.WriteLine("\te) Set Alarm");
                Console.WriteLine("\tq) Return to main menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        // User Validation NEEDED
                        sensorsToMonitor = LightAlarmDisplaySetSensorsToMonitor();
                        break;

                    case "b":
                        rangeType = LightAlarmDisplaySetRangeType();
                        break;

                    case "c":
                        minMaxValue = LightAlarmSetMinMaxThreshold(rangeType, finchRobot);
                        break;

                    case "d":
                        timeToMonitor = LightAlarmSetTime();
                        break;

                    case "e":
                        lightAlarmSetAlarm(finchRobot, sensorsToMonitor, rangeType, minMaxValue, timeToMonitor);
                        break;
                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;

                }
            } while (!quitMenu);
        }

        private static void lightAlarmSetAlarm
            (Finch finchRobot, 
            string sensorsToMonitor, 
            string rangeType, 
            int minMaxValue, 
            int timeToMonitor)
        {

            int secondsElapsed = 0;
            bool thresholdExceeded = false;
            int currentLightSensorValue = 0;
            DisplayScreenHeader("Set Alarm");

            Console.WriteLine($"\tSensors to Monitor: {sensorsToMonitor}");
            Console.WriteLine($"\tRange Type: {rangeType}");
            Console.WriteLine($"\tMin/Max Value: {minMaxValue}");
            Console.WriteLine($"\tTime To Monitor: {timeToMonitor}");
            Console.WriteLine();
            Console.WriteLine("Press any Key to Begin: ");
            Console.ReadKey();
            Console.WriteLine();


            while ((secondsElapsed < timeToMonitor) && !thresholdExceeded)
            {

                switch (sensorsToMonitor)
                {
                    case "left":
                        currentLightSensorValue = finchRobot.getLeftLightSensor();
                        break;

                    case "right":
                        currentLightSensorValue = finchRobot.getRightLightSensor();
                        break;

                    case "both":

                        currentLightSensorValue = (finchRobot.getLeftLightSensor() + finchRobot.getRightLightSensor()) / 2;
                        break;

                }

                switch (rangeType)
                {
                    case "minimum":
                        if (currentLightSensorValue < minMaxValue)
                        {

                            thresholdExceeded = true;

                        }
                        break;

                    case "maximum":
                        if (currentLightSensorValue > minMaxValue)
                        {

                            thresholdExceeded = true;

                        }
                        break;
                }
                finchRobot.wait(1000);
                secondsElapsed++;
            }

            if (thresholdExceeded)
            {
                finchRobot.setLED(0, 255, 0);
                finchRobot.noteOn(2000);
                finchRobot.wait(1000);
                finchRobot.noteOn(1600);
                finchRobot.wait(500);
                finchRobot.noteOn(800);
                finchRobot.wait(1000);
                finchRobot.noteOff();
                finchRobot.setLED(0, 0, 0);
                Console.WriteLine($"The {rangeType} threshold value of {minMaxValue} has been exceeded by the current light sensors with a value of {currentLightSensorValue}.");

            }
            else
            {
                finchRobot.setLED(255, 0, 0);
                finchRobot.wait(3000);
                finchRobot.setLED(0, 0, 0);
                Console.WriteLine($"The {rangeType} threshold value of {minMaxValue} was not exceeded.");
            }

            DisplayMenuPrompt("Light Alarm");
        }

        static int LightAlarmSetTime()
        {
            int timeToMonitor = 0;
            bool valid = false;
            string answer;
            DisplayScreenHeader("Time to Monitor");


            // user validation
            while (valid != true)
            {
                Console.WriteLine("Time to monitor: ");
                answer = Console.ReadLine();
                if (int.TryParse(answer, out timeToMonitor) != true)
                {
                    Console.WriteLine("Error! please enter a valid number!\n");
                }
                else
                {
                    valid = true;
                    break;
                }
            }
            DisplayMenuPrompt("Light Alarm");
            return timeToMonitor;
        }
        static int LightAlarmSetMinMaxThreshold(string rangeType, Finch finchRobot)
        {
            int minMaxValue;
            minMaxValue = 0;
            bool valid = false;
            string answer;
            DisplayScreenHeader("Minimum/Maximum Threshold");

            Console.WriteLine($"\tLeft Light Sensor Ambient Value: {finchRobot.getLeftLightSensor()}");
            Console.WriteLine($"\tRight Light Sensor Ambient Value: {finchRobot.getRightLightSensor()}");
            Console.WriteLine();
            // user validation
            while (valid != true)
            {
                Console.WriteLine($"Enter the {rangeType} Light Sensor Value:");
                answer = Console.ReadLine();
                if (int.TryParse(answer, out minMaxValue) != true)
                {
                    Console.WriteLine("Error! please enter a valid number!\n");
                }
                else
                {
                    valid = true;
                    break;
                }
            }
                DisplayMenuPrompt("Light Alarm");

            return minMaxValue;
        }

        /// <summary>
        /// Gets input to see which sensor to monitor!
        /// </summary>
        /// <returns>User selected sensor</returns>
        static string LightAlarmDisplaySetSensorsToMonitor()
        {
            string sensorsToMonitor;
            sensorsToMonitor = null;
            bool valid = false;
            string[] answers = { "left", "right", "both" };
            DisplayScreenHeader("Sensors To Monitor");
            // DETECTS TO SEE IF ANSWER IS VALID
           while (valid != true)
            {
                Console.WriteLine("\tSensors to Monitor [left, right, both]:");
                sensorsToMonitor = Console.ReadLine();
                if (answers.Contains(sensorsToMonitor) != true)
                {
                    Console.WriteLine("Error: Answer is not valid!\nPlease Select a Valid Answer!");
                }
                else
                {
                    valid = true;
                    break;
                }
            }
            DisplayMenuPrompt("Light Alarm");

            return sensorsToMonitor;
        }

        static string LightAlarmDisplaySetRangeType()
        {
            string rangeType;
            rangeType = null;
            bool valid = false;
            string[] answers = { "minimum", "maximum" };

            DisplayScreenHeader("Set Range Type");
            // DETECTS TO SEE IF ANSWER IS VALID
            while (valid != true)
            {
                Console.WriteLine("\tSet Range Type[Minimum or Maximum]:");
                rangeType = Console.ReadLine();
                if (answers.Contains(rangeType) != true)
                {
                    Console.WriteLine("Error: Answer is not valid!\nPlease Select a Valid Answer!");
                }
                else
                {
                    valid = true;
                    break;
                }
            }
            DisplayMenuPrompt("Light Alarm");

            return rangeType;
        }

        #endregion

        #region DATA RECORDER

        static void dataRecorderDisplayMenuScreen(Finch finchRobot)
        {
            int numberOfDataPoints = 0;
            double dataPointFrequency = 0;
            double[] temperatures = null;

            Console.CursorVisible = true;

            bool quitMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Data Recorder Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Number of Data Points");
                Console.WriteLine("\tb) Frequency of Data Points");
                Console.WriteLine("\tc) Get Data");
                Console.WriteLine("\td) Show Data");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        // Collects Number of Data points user would like
                        numberOfDataPoints = DataRecorderDisplayGetNumberOfDataPoints();
                        break;

                    case "b":
                        dataPointFrequency = DataRecorderDisplayGetDataPointFrequency();
                        break;

                    case "c":
                        temperatures = DataRecorderDisplayGetData(numberOfDataPoints, dataPointFrequency, finchRobot);
                        break;

                    case "d":
                        DataRecorderDisplayData(temperatures);
                        break;

                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;

                }
            } while (!quitMenu);

        }
        /// <summary>
        /// Displays Recieved Data in a table for user
        /// </summary>
        /// <param name="temperatures"></param>
        static void DataRecorderDisplayData(double[] temperatures)
        {
            DisplayScreenHeader("Showing Data:");

            DataReadingDisplayTable(temperatures);

            DisplayContinuePrompt();
        }

        static void DataReadingDisplayTable(double[] temperatures)
        {

            //
            // display table headers
            //
            Console.WriteLine(
                "Recording #".PadLeft(15) +
                "Temp".PadLeft(15)
                );
            Console.WriteLine(
                "...........".PadLeft(15) +
                "-----------".PadLeft(15)
                );

            //
            // displays table data
            //
            for (int i = 0; i < temperatures.Length; i++)
            {
                Console.WriteLine(
                    (i + 1).ToString().PadLeft(15) +
                    temperatures[i].ToString("n2") + "°f" + "".PadLeft(15) 
                    );
            }
            Console.WriteLine();
            Console.WriteLine("The Average of your temperatures was: " + temperatures.Average().ToString("n2")+ "°f");
        }

        /// <summary>
        /// Records Data of Surrounding temperature of Finch Robot
        /// </summary>
        /// <param name="numberOfDataPoints"></param>
        /// <param name="dataPointFrequency"></param>
        /// <param name="finchRobot"></param>
        /// <returns>Records Temp. of surrounding area around Finch</returns>
        static double[] DataRecorderDisplayGetData(int numberOfDataPoints, double dataPointFrequency, Finch finchRobot)
        {
            double[] temperatures = new double[numberOfDataPoints];

            DisplayScreenHeader("Get Data");

            Console.WriteLine($"\tNumber of data points: {numberOfDataPoints}");
            Console.WriteLine($"\tData Point Frequency: {dataPointFrequency}");
            Console.WriteLine();
            Console.WriteLine("\tThe Finch Robot is ready to begin recording the temperature data.");
            DisplayContinuePrompt();

            for (int i = 0; i < numberOfDataPoints; i++)
            {
                double preSwappedTemp = 0;

                //conversionCtoF(preSwappedTemp, finchRobot);
                temperatures[i] = (finchRobot.getTemperature() * 1.8) + 32;
                Console.WriteLine($"\tReading {i + 1}   :   {temperatures[i].ToString("n2")}°f" );
                int waitInSeconds = (int)(dataPointFrequency * 1000);
                finchRobot.wait(waitInSeconds);
            }

            Console.WriteLine();
            Console.WriteLine("\tTable of Temperatures");
            Console.WriteLine();
            DataReadingDisplayTable(temperatures);
            DisplayContinuePrompt();
            return temperatures;
        }
        /// <summary>
        /// Uses conversion math to convert C to F for user
        /// </summary>
        /// <param name="preSwappedTemp"></param>
        /// <param name="finchRobot"></param>
        /// <returns>conversion math to convert C to F for user</returns>
        static double conversionCtoF(double preSwappedTemp, Finch finchRobot)
        {
            preSwappedTemp = (finchRobot.getTemperature() * 1.8) + 32;
            return preSwappedTemp;
        }

        /// <summary>
        /// Gets frequency input from user
        /// </summary>
        /// <returns>Frequency input from user</returns>
        static double DataRecorderDisplayGetDataPointFrequency()
        {
            double dataPointFrequency;
            bool answer = false;
            string userResponse;
            int userAnswer;
            int num = -1;
            DisplayScreenHeader("Frequency of Data points");
            Console.WriteLine("Frequency of data points: ");

            //ASKS FOR USER INPUT
            Console.WriteLine("How many seconds would you like between each reading? (can be a decimal!)");
            userResponse = Console.ReadLine();

            // DETECTS TO SEE IF ANSWER IS VALID
            answer = double.TryParse(userResponse, out dataPointFrequency);
            if (answer != true)
            {
                while (answer != true)
                {
                    Console.Clear();
                    Console.WriteLine("Error! please enter a valid number!\n");
                    Console.WriteLine("How many seconds would you like between each reading? (whole number)");
                    // USER INPUT 
                    userResponse = Console.ReadLine();
                    // validate user input
                    answer = double.TryParse(userResponse, out dataPointFrequency);

                }
            }
            // RETURN VALUE
            return dataPointFrequency;
        }

        /// <summary>
        /// Get's the number of data points from user
        /// </summary>
        /// <returns># of data points</returns>
        static int DataRecorderDisplayGetNumberOfDataPoints()
        {
            int numberOfDataPoints;
            bool answer = false;
            string userResponse;
            int userAnswer;
            int num = -1;
            DisplayScreenHeader("Number of Data Points");
            Console.WriteLine("Number of data points: ");

            //ASKS FOR USER INPUT
            Console.WriteLine("How many data points would you like? (whole number)");
            userResponse = Console.ReadLine();

            // DETECTS TO SEE IF ANSWER IS VALID
            answer = int.TryParse(userResponse, out numberOfDataPoints);
            if (answer != true)
            {
                while (answer != true)
                {
                    Console.Clear();
                    Console.WriteLine("Error! please enter a valid whole number!\n");
                    Console.WriteLine("How many data points would you like? (whole number)");
                    // USER INPUT 
                    userResponse = Console.ReadLine();
                    // validate user input
                    answer = int.TryParse(userResponse, out numberOfDataPoints);

                }
            }
            // RETURN VALUE
            return numberOfDataPoints;
        }


        #endregion
        #region FINCH ROBOT MANAGEMENT

        /// <summary>
        /// *****************************************************************
        /// *               Disconnect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Disconnect Finch Robot");

            Console.WriteLine("\tAbout to disconnect from the Finch robot.");
            DisplayContinuePrompt();

            finchRobot.disConnect();

            Console.WriteLine("\tThe Finch robot is now disconnect.");

            DisplayMenuPrompt("Main Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *                  Connect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            bool robotConnected;

            DisplayScreenHeader("Connect Finch Robot");

            Console.WriteLine("\tAbout to connect to Finch robot. Please be sure the USB cable is connected to the robot and computer now.");
            DisplayContinuePrompt();

            robotConnected = finchRobot.connect();
            finchRobot.connect();
            // TODO test connection and provide user feedback - text, lights, sounds
            finchRobot.noteOn( 300);
            finchRobot.setLED(0, 0, 255);
            finchRobot.wait(4000);
            DisplayMenuPrompt("Main Menu");

            //
            // reset finch robot
            //
            finchRobot.setLED(0, 0, 0);
            finchRobot.noteOff();

            return robotConnected;
        }

        #endregion

        #region USER INTERFACE

        /// <summary>
        /// *****************************************************************
        /// *                     Welcome Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tFinch Control");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Closing Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Finch Control!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display menu prompt
        /// </summary>
        static void DisplayMenuPrompt(string menuName)
        {
            Console.WriteLine();
            Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion


    }
    
}
