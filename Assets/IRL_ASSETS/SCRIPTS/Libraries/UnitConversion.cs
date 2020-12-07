using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using HomaGames.Internal.Utilities;

public class UnitConversion
{
    private static StringFast stringFast = new StringFast();

    private static string unit;
    private static string cashstring;

    public static string ToSeconds(float value)

    {
        stringFast.Clear();

        value = (float)Math.Round(value, 1);
        stringFast.Append(value);
        stringFast.Append("s");
        return stringFast.ToString();
    }

    public static string ToSecondsWithRatio(float value)

    {
        stringFast.Clear();

        value = (float)Math.Round(value, 1);
        stringFast.Append(value);
        stringFast.Append("/s");
        return stringFast.ToString();
    }

    public static string RoundAndString(float value)

    {
        stringFast.Clear();

        value = (float)Math.Round(value, 1);
        stringFast.Append(value);
        return stringFast.ToString();
    }

    public static string CashUnitWithoutDollars(float playerCash)
    {
        unit = "";

        float divide = 1;
        float returncash = playerCash;

        if (playerCash >= 1000)
        {
            unit = "K";
            divide = 1000;

            if (playerCash >= 1000000f)
            {
                unit = "M";
                divide = 1000000f;

                if (playerCash >= 1000000000f)
                {
                    unit = "B";
                    divide = 1000000000f;

                    if (playerCash >= 1000000000000f)
                    {
                        unit = "T";
                        divide = 1000000000000f;

                        if (playerCash >= 1000000000000000f)
                        {
                            unit = "AA";
                            divide = 1000000000000000f;

                            if (playerCash >= 1000000000000000000f)
                            {
                                unit = "BB";
                                divide = 1000000000000000000f;

                                if (playerCash >= 1000000000000000000000f)
                                {
                                    unit = "CC";
                                    divide = 1000000000000000000000f;

                                    if (playerCash >= 1000000000000000000000000f)
                                    {
                                        unit = "DD";
                                        divide = 1000000000000000000000000f;

                                        if (playerCash >= 1000000000000000000000000000f)
                                        {
                                            unit = "EE";
                                            divide = 1000000000000000000000000000f;

                                            if (playerCash >= 1000000000000000000000000000000f)
                                            {
                                                unit = "FF";
                                                divide = 1000000000000000000000000000000f;

                                                if (playerCash >= 1000000000000000000000000000000000f)
                                                {
                                                    unit = "GG";
                                                    divide = 1000000000000000000000000000000000f;

                                                    if (playerCash >= 1000000000000000000000000000000000000f)
                                                    {
                                                        unit = "HH";
                                                        divide = 1000000000000000000000000000000000000f;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        stringFast.Clear();

        returncash = (playerCash / divide);
        returncash = (float)Math.Round(returncash, 1);

        stringFast.Append(returncash);
        stringFast.Append(unit);

        return stringFast.ToString();
    }

    public static string CashUnit(float playerCash, bool displayDollardSymbol = false)
    {
        unit = "";

        float divide = 1;
        float returncash = playerCash;

        if (playerCash >= 1000)
        {
            unit = "K";
            divide = 1000;

            if (playerCash >= 1000000f)
            {
                unit = "M";
                divide = 1000000f;

                if (playerCash >= 1000000000f)
                {
                    unit = "B";
                    divide = 1000000000f;

                    if (playerCash >= 1000000000000f)
                    {
                        unit = "T";
                        divide = 1000000000000f;

                        if (playerCash >= 1000000000000000f)
                        {
                            unit = "AA";
                            divide = 1000000000000000f;

                            if (playerCash >= 1000000000000000000f)
                            {
                                unit = "BB";
                                divide = 1000000000000000000f;

                                if (playerCash >= 1000000000000000000000f)
                                {
                                    unit = "CC";
                                    divide = 1000000000000000000000f;

                                    if (playerCash >= 1000000000000000000000000f)
                                    {
                                        unit = "DD";
                                        divide = 1000000000000000000000000f;

                                        if (playerCash >= 1000000000000000000000000000f)
                                        {
                                            unit = "EE";
                                            divide = 1000000000000000000000000000f;

                                            if (playerCash >= 1000000000000000000000000000000f)
                                            {
                                                unit = "FF";
                                                divide = 1000000000000000000000000000000f;

                                                if (playerCash >= 1000000000000000000000000000000000f)
                                                {
                                                    unit = "GG";
                                                    divide = 1000000000000000000000000000000000f;

                                                    if (playerCash >= 1000000000000000000000000000000000000f)
                                                    {
                                                        unit = "HH";
                                                        divide = 1000000000000000000000000000000000000f;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        stringFast.Clear();

        returncash = (playerCash / divide);
        returncash = (float)Math.Round(returncash, 1);

        if (displayDollardSymbol)
        {
            stringFast.Append("$");
            stringFast.Append(returncash);
            stringFast.Append(unit);
        }
        else
        {
            stringFast.Append(returncash);
            stringFast.Append(unit);
        }

        return stringFast.ToString();
    }

    public static float ReturnNumberFromUnit(string unit, float number)
    {
        switch (unit)
        {
            case "":
                break;
            case "K":
                number *= 1000f;
                break;
            case "M":
                number *= 1000000f;
                break;
            case "B":
                number *= 1000000000f;
                break;
            case "T":
                number *= 1000000000000f;
                break;
            case "AA":
                number *= 1000000000000000f;
                break;
            case "BB":
                number *= 1000000000000000000f;
                break;
            case "CC":
                number *= 1000000000000000000000f;
                break;
            case "DD":
                number *= 1000000000000000000000000f;
                break;
            case "EE":
                number *= 1000000000000000000000000000f;
                break;
            case "FF":
                number *= 1000000000000000000000000000000f;
                break;
            case "GG":
                number *= 1000000000000000000000000000000000f;
                break;
            case "HH":
                number *= 1000000000000000000000000000000000000f;
                break;
            default:
                Debug.LogError("Unit string syntaxe don't correspond");
                break;
        }

        return number;
    }


}
