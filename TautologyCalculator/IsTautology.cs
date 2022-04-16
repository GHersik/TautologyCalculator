using System;
using System.Collections.Generic;
using System.Text;

namespace TautologyCalculator
{
    public class IsTautology
    {
        //Dane wejściowe i długość stringa
        private readonly string inputData;
        private readonly int inputLength;

        //Lista argumentów bez powtarzających się, oraz ich ilość
        private List<char> argumentsListChar = new List<char>();
        private int argCount;
        private int argPossibilitesCount = 2;

        //Tablica logiczna
        private string[,] matrix;

        //Tablica wyjściowa, do odczytu na końcu
        public string[,] matrixOut;

        //Zmienne używane w pętlach do ewaluowania wyrażenia
        private int iterate = 0;
        private int globalIterate = 0;

        public IsTautology(string Input)
        {
            inputData = Input;
            inputLength = Input.Length;
        }

        /// <summary>
        /// Metoda sprawdzająca dane wejściowe i zbierająca na listę wszystkie argumenty
        /// które się nie powtarzają w danych wejściowych. Sortuje je, następnie oblicza
        /// wielkość tablicy logicznej
        /// </summary>
        public void ArgumentsCheck()
        {
            for (int i = 0; i < inputLength; i++)
            {
                if (char.IsLetter(inputData[i]))
                {
                    if (!argumentsListChar.Contains(inputData[i]))
                    {
                        argumentsListChar.Add(inputData[i]);
                    }
                }
            }
            argumentsListChar.Sort();
            argCount = argumentsListChar.Count;
            for (int i = 1; i < argCount; i++)
            {
                argPossibilitesCount *= 2;
            }
        }


        /// <summary>
        /// Metoda generująca tablicę logiczną oraz inicjująca tablicę wyjściową
        /// </summary>
        public void GeneratePossibilityMatrix()
        {
            matrix = new string[argCount, argPossibilitesCount];
            int outerloop = argPossibilitesCount / 2;
            int innerloop = 0;
            string dataBool = "0";

            for (int i = 0; i < argCount; i++)
            {
                for (int n = 0; n < argPossibilitesCount; n++)
                {
                    if (outerloop <= innerloop)
                    {
                        dataBool = "1";
                        if (innerloop >= outerloop * 2)
                        {
                            dataBool = "0";
                            innerloop = 0;
                        }
                    }
                    matrix[i, n] = dataBool;
                    innerloop += 1;
                }
                outerloop /= 2;
            }

            matrixOut = new string[argPossibilitesCount, inputLength];
        }

        /// <summary>
        /// Metoda która inicjuje ewaluację wyrażenia wejściowego w dwóch pętlach które
        /// korzystają z dwóch globalnych zmiennych
        /// </summary>
        public void OperatorEvaluation()
        {
            while (globalIterate < argPossibilitesCount)
            {
                for (; iterate < inputLength;)
                {
                    switch (inputData[iterate])
                    {
                        case '(':
                            matrixOut[globalIterate, iterate] = " ";
                            iterate++;
                            Proposition();
                            break;
                        case ' ':
                            matrixOut[globalIterate, iterate] = " ";
                            iterate++;
                            break;
                        default:
                            int indexOf = 0;
                            for (int i = 0; i < argCount; i++)
                            {
                                if (inputData[iterate] == argumentsListChar[i])
                                {
                                    indexOf = i;
                                    break;
                                }
                            }
                            matrixOut[globalIterate, iterate] = matrix[indexOf, globalIterate];
                            iterate++;
                            break;
                    }
                }
                iterate = 0;
                globalIterate++;
            }
        }

        /// <summary>
        /// Metoda która rekurencyjnie odwołuje się sama do siebie o ile takie odwołanie istnieje
        /// w danych wejściowych
        /// </summary>
        /// <returns></returns>
        public string Proposition()
        {
            //Argumenty do przypisania
            string firstArgument = "0";
            string secondArgument = "0";

            //koniec pętli pierwszej bądź drugiej
            bool isFirstEvaluated = false;
            bool isSecondEvaluated = false;

            //numer konkretnego operatora -1 default, 0 Negacja, 1 koniunkcja,
            //2 alternatywa, 3 implikacja, 4 równość
            int operatorsNo = -1;

            //indeks operatora w zdaniu
            int indexOfOperator = 0;

            //pierwsza pętla
            for (; iterate < inputLength;)
            {
                if (isFirstEvaluated == true)
                {
                    break;
                }
                switch (inputData[iterate])
                {
                    case '(':
                        matrixOut[globalIterate, iterate] = " ";
                        iterate++;
                        firstArgument = Proposition();
                        isFirstEvaluated = true;
                        break;
                    case ' ':
                        matrixOut[globalIterate, iterate] = " ";
                        iterate++;
                        break;
                    case '!':
                        operatorsNo = 0;
                        indexOfOperator = iterate;
                        iterate++;
                        break;
                    default:
                        int indexOf = 0;
                        for (int i = 0; i < argCount; i++)
                        {
                            if (inputData[iterate] == argumentsListChar[i])
                            {
                                indexOf = i;
                                break;
                            }
                        }
                        firstArgument = matrix[indexOf, globalIterate];
                        matrixOut[globalIterate, iterate] = firstArgument;
                        isFirstEvaluated = true;
                        iterate++;
                        break;
                }
            }

            //druga pętla
            for (; iterate < inputLength;)
            {
                if (isSecondEvaluated == true)
                {
                    break;
                }
                switch (inputData[iterate])
                {
                    case '(':
                        matrixOut[globalIterate, iterate] = " ";
                        iterate++;
                        secondArgument = Proposition();
                        break;
                    case ')':
                        matrixOut[globalIterate, iterate] = " ";
                        iterate++;
                        isSecondEvaluated = true;
                        break;
                    case ' ':
                        matrixOut[globalIterate, iterate] = " ";
                        iterate++;
                        break;
                    case '&':
                        operatorsNo = 1;
                        indexOfOperator = iterate;
                        iterate++;
                        break;
                    case '|':
                        operatorsNo = 2;
                        indexOfOperator = iterate;
                        iterate++;
                        break;
                    case '-':
                        matrixOut[globalIterate, iterate] = " ";
                        operatorsNo = 3;
                        iterate++;
                        for (; iterate < inputLength; iterate++)
                        {
                            if (inputData[iterate] == '-')
                            {
                                indexOfOperator = iterate;
                            }
                            else if (inputData[iterate] == '>')
                            {
                                matrixOut[globalIterate, iterate] = " ";
                                iterate++;
                                break;
                            }
                            else
                            {
                                matrixOut[globalIterate, iterate] = " ";
                            }
                        }
                        break;
                    case '<':
                        matrixOut[globalIterate, iterate] = " ";
                        operatorsNo = 4;
                        iterate++;
                        for (; iterate < inputLength; iterate++)
                        {
                            if (inputData[iterate] == '-')
                            {
                                indexOfOperator = iterate;
                            }
                            else if (inputData[iterate] == '>')
                            {
                                matrixOut[globalIterate, iterate] = " ";
                                iterate++;
                                break;
                            }
                            else
                            {
                                matrixOut[globalIterate, iterate] = " ";
                            }
                        }
                        break;
                    default:
                        int indexOf = 0;
                        for (int i = 0; i < argCount; i++)
                        {
                            if (inputData[iterate] == argumentsListChar[i])
                            {
                                indexOf = i;
                                break;
                            }
                        }
                        secondArgument = matrix[indexOf, globalIterate];
                        matrixOut[globalIterate, iterate] = secondArgument;
                        iterate++;
                        break;
                }
            }

            //wynik końcowy
            switch (operatorsNo)
            {
                case 0:
                    if (firstArgument == "0")
                    {
                        matrixOut[globalIterate, indexOfOperator] = "1";
                        return "1";
                    }
                    else
                    {
                        matrixOut[globalIterate, indexOfOperator] = "0";
                        return "0";
                    }
                case 1:
                    if (firstArgument == secondArgument && firstArgument == "1")
                    {
                        matrixOut[globalIterate, indexOfOperator] = "1";
                        return "1";
                    }
                    else
                    {
                        matrixOut[globalIterate, indexOfOperator] = "0";
                        return "0";
                    }
                case 2:
                    if (firstArgument == secondArgument && firstArgument == "0")
                    {
                        matrixOut[globalIterate, indexOfOperator] = "0";
                        return "0";
                    }
                    else
                    {
                        matrixOut[globalIterate, indexOfOperator] = "1";
                        return "1";
                    }
                case 3:
                    if (firstArgument == "1" && secondArgument == "0")
                    {
                        matrixOut[globalIterate, indexOfOperator] = "0";
                        return "0";
                    }
                    else
                    {
                        matrixOut[globalIterate, indexOfOperator] = "1";
                        return "1";
                    }
                case 4:
                    if (firstArgument == secondArgument)
                    {
                        matrixOut[globalIterate, indexOfOperator] = "1";
                        return "1";
                    }
                    else
                    {
                        matrixOut[globalIterate, indexOfOperator] = "0";
                        return "0";
                    }
                default:
                    matrixOut[globalIterate, indexOfOperator] = " ";
                    return " ";
            }
        }
    }
}