using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Summary description for PairwiseScrambler
/// </summary>
namespace obfuscate
{
    public class PairwiseScrambler : IScrambler
    {
        private char[] charArray = { 'a', 'b', 'c', 'ç', 'd', 'e', 'f', 'g', 'ğ', 'h', 'ı', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'ö', 'p', 'r', 's', 't', 'u', 'ü', 'v', 'w', 'x', 'y', 'z', 'q', 'A', 'B', 'C', 'Ç', 'D', 'E', 'F', 'G', 'Ğ', 'H', 'I', 'İ', 'J', 'K', 'L', 'M', 'N', 'O', 'Ö', 'P', 'R', 'S', 'T', 'U', 'Ü', 'V', 'W', 'X', 'Y', 'Q', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '/', '=', ':', ';', '?', '[', ']', ' ', '<', '>', '!', '_', '-', '.', '"', '\r', '\n', '\t', '+', ',', '&', '\u00A0', '\'', '(', ')', '{', '}', '|', '%', '#', '•', '@', '©', 'ş', 'Ş', '$', '\\' };
        private string[] stringArray = { "q19", "er6", "as2", "ty6", "ot7", "mn1", "k9p", "n11", "w33", "zma", "i0i", "u89", "xox", "xxx", "ooo", "o0o", "jav", "b35", "b41", "g22", "d5t", "ddd", "hr8", "l99", "pe7", "QeR", "Yuv", "YUV", "O89", "W42", "WTr", "ER7", "IOO", "QyU", "R85", "Ccv", "AAl", "Tyy", "TTh", "Pok", "KsM", "KSE", "XZc", "XYZ", "VBN", "VTR", "HSh", "Ddf", "F5d", "Bab", "Faf", "FT7", "Fhj", "Ld7", "Mno", "Ma3", "Muy", "Mo5", "DFc", "Nu2", "Oop", "Ge9", "GeG", "UYU", "USL", "UTK", "SDD", "TOK", "KOT", "T4c", "Cnb", "Bxx", "LkJ", "ISS", "QWE", "RPC", "Qkv", "Shp", "Db5", "Jjj", "Uom", "U17", "Hoo", "WsB", "NVC", "MVC", "VvN", "Bii", "B5d", "Ax2", "Ty8", "UML", "Bak", "LAz", "LOt", "MaY", "DoM", "DUk", "siz", "SHp", "MsW", "CcW", "Wcs", "ATj", "COp", "SvB", "LvB", "Dol" };
        string charArrayName = "", stringArrayName = "", contentStringName = "";
        string findStrMethodName, decodeMethodName, indexString,mainPart="";
        string charArrStr = "", strinArrStr = "";
        int stepKey = 2;
        int sizeKey;
        //private Dictionary<int,string> interDic= new Dictionary<int,string>();
        private Dictionary<int, Fake> methodDic = new Dictionary<int, Fake>();

        public PairwiseScrambler()
        {
            long seedL = DateTime.Now.Ticks;
            long see = seedL % 1000000;
            int seedInt = Convert.ToInt32(see);
            Random r = new Random(seedInt);
            stepKey = r.Next(2, 100);
            sizeKey = r.Next(0, 2) + 1;
            charArrayName = GRS(r.Next(9, 14)) + GRS(r.Next(7, 12));
            stringArrayName = GRS(r.Next(9, 14)) + "" + r.Next(8, 50);

            StringBuilder js = new StringBuilder();
            js.Append("var " + charArrayName + "=[");
            string str = @"'a','b','c','ç','d','e','f','g','ğ','h','ı','i','j','k','l','m','n','o','ö','p','r','s','t','u','ü','v','w','x','y','z','q','A','B','C','Ç','D','E','F','G','Ğ','H','I','İ','J','K','L','M','N','O','Ö','P','R','S','T','U','Ü','V','W','X','Y','Q','Z','0','1','2','3','4','5','6','7','8','9','/','=',':',';','?','[',']',' ','<', '>','!','_','-','.',";
            js.Append(str);
            js.Append("'\"',");
            js.Append(@"'\r', '\n','\t','+',',','&','\u00A0','\'','(',')','{','}','|','%','#','•','@','©','ş','Ş','$'];");
            charArrStr = js.ToString();
            js = new StringBuilder();
            
            stringArrayName = GRS(r.Next(4, 14)) + "" + r.Next(4, 50);
            js.Append("var " + stringArrayName + "=[");
            string arrStr = "\"q19\",\"er6\",\"as2\",\"ty6\",\"ot7\",\"mn1\",\"k9p\",\"n11\",\"w33\",\"zma\",\"i0i\",\"u89\",\"xox\",\"xxx\",\"ooo\",\"o0o\",\"jav\",\"b35\",\"b41\",\"g22\",\"d5t\",\"ddd\",\"hr8\",\"l99\",\"pe7\",\"QeR\",\"Yuv\",\"YUV\",\"O89\",\"W42\",\"WTr\",\"ER7\",\"IOO\",\"QyU\",\"R85\",\"Ccv\",\"AAl\",\"Tyy\",\"TTh\",\"Pok\",\"KsM\",\"KSE\",\"XZc\",\"XYZ\",\"VBN\",\"VTR\",\"HSh\",\"Ddf\",\"F5d\",\"Bab\",\"Faf\",\"FT7\",\"Fhj\",\"Ld7\",\"Mno\",\"Ma3\",\"Muy\",\"Mo5\",\"DFc\",\"Nu2\",\"Oop\",\"Ge9\",\"GeG\",\"UYU\",\"USL\",\"UTK\",\"SDD\",\"TOK\",\"KOT\",\"T4c\",\"Cnb\",\"Bxx\",\"LkJ\",\"ISS\",\"QWE\",\"RPC\",\"Qkv\",\"Shp\",\"Db5\",\"Jjj\",\"Uom\",\"U17\",\"Hoo\",\"WsB\",\"NVC\",\"MVC\",\"VvN\",\"Bii\",\"B5d\",\"Ax2\",\"Ty8\",\"UML\",\"Bak\",\"LAz\",\"LOt\",\"MaY\",\"DoM\",\"DUk\",\"siz\",\"SHp\",\"MsW\",\"CcW\",\"Wcs\",\"ATj\",\"COp\",\"SvB\",\"LvB\",\"Dol\"";
            js.Append(arrStr);
            js.Append("];");
            strinArrStr = js.ToString();
            
            findStrMethodName = GRS(r.Next(3, 15)) + "" + r.Next(11, 17);
            decodeMethodName = GRS(r.Next(2, 11)) + "" + r.Next(4, 11);
            indexString = GRS(r.Next(1, 8)) + "" + r.Next(1, 9) + "" + GRS(r.Next(5, 11));
            contentStringName = GRS(r.Next(3, 13)) + GRS(r.Next(3, 13));

            methodDic.Add(0, new FakeMethod(decodeMethodName, GetR0Method()));
            methodDic.Add(1, new FakeMethod(GRS(r.Next(4, 12)), GetR1Method()));
            methodDic.Add(2, new FakeMethod(GRS(r.Next(6, 18)), GetR2Method()));
            methodDic.Add(3, new FakeMethod(GRS(r.Next(3, 13)), GetR3Method()));
            methodDic.Add(4, new FakeMethod(GRS(r.Next(3, 14)), GetDummyMethod()));
            methodDic.Add(5, new FakeString(DecodeMethod()));

            string v0Str = "";//gercek method ekleniyor
            if (DateTime.Now.Millisecond > 500)
                v0Str += "var " + GRS(r.Next(3, 6)) + "='" + GRS(r.Next(1, 5)) + "';";
            if (DateTime.Now.Minute % 2 == 0)
                v0Str += " { ";
            if (DateTime.Now.Millisecond % 100 > 50)
                v0Str += " function " + GRS(r.Next(3, 5)) + "() {document.write('" + GRS(r.Next(1, 3)) + "')}";
           
            if (DateTime.Now.Minute % 2 == 0)
                v0Str += " } ";
            v0Str += "document.write(" + methodDic[0].MethodName + "());";
            methodDic.Add(6, new FakeString(v0Str));            
                        
            //fake1 ekleniyor 
            string v1Str = "";
            if (DateTime.Now.Millisecond > 500)
                v1Str += "var " + GRS(r.Next(3, 6)) + "='" + GRS(r.Next(1, 5)) + "';";
            //if (DateTime.Now.Millisecond % 100 > 50)
            //    v1Str += " function " + GRS(r.Next(3, 5)) + "() {document.write('" + GRS(r.Next(1, 3)) + "')}";
            if (DateTime.Now.Minute % 2 == 0)
                v1Str += " { var functionn=0;";

            v1Str += "var " + GRS(r.Next(5, 8)) + "='{ var " + GRS(r.Next(3, 5)) + "=" + GRS(r.Next(3, 5))
                + " ; document.write(" + methodDic[2].MethodName + "(" + contentStringName + "))';";
            
            if (DateTime.Now.Minute % 2 == 0)
                v1Str += " } ";
            
            methodDic.Add(7, new FakeString(v1Str));
            
            //fake2 ekleniyor 
            methodDic.Add(8, new FakeString(" {var " + GRS(r.Next(5, 8)) + "='{ var " + GRS(r.Next(3, 5)) + "=" + GRS(r.Next(3, 5))
                + " ; document.write(" + methodDic[2].MethodName + "(" + contentStringName + "))';}"));

            //fake3 fake ekleniyor;
           string v3Str = "";
           v3Str += " { var " + GRS(r.Next(5, 8)) + "='{ var " + GRS(r.Next(3, 5)) + "=" + r.Next(3, 5)+";";
           if (DateTime.Now.Second < 20) v3Str += " document.write(    );";
            v3Str  += "document.write(" + methodDic[3].MethodName + "())';}";
            methodDic.Add(9, new FakeString(v3Str));

           

        }       

        #region IScrambler Members

        public string encode(char[] HTML)
        {
            StringBuilder decodedHTML = new StringBuilder();
            //byte currentIndex = 0, nextIndex=0;

            int count = 0;
            for (int i = 0; i < HTML.Length; i++)
            {

                if (count > 0)
                {
                    count--;
                    continue;
                }

                if (HTML[i].Equals('&') && (i <= HTML.Length - 6))
                {
                    string unsupportedString = "";
                    for (int k = 0; k < 6; k++)
                    {
                        unsupportedString += HTML[i + k];
                    }
                    if (unsupportedString.Equals("&nbsp;"))
                    {
                        count = 5;
                    }
                }
                byte index;
                if (count == 5)
                {
                    index = findCharacter('\u00A0');
                }
                else
                {
                    index = findCharacter(HTML[i]);
                }
                decodedHTML.Append(stringArray[(index + stepKey) % (stringArray.Length)] + GRS(sizeKey));

            }

            //methodDic.Add(3, createDecoder(decodedHTML.ToString()));          

            return createDecoder(decodedHTML.ToString());
            //+ GetR1Method() + GetR2Method() + DecodeMethod()

            //return decodedHTML.ToString();
        }

        public string GRS(int size)
        {

            long seedL = DateTime.Now.Ticks;
            long see = seedL % 1000000;
            int seedInt = Convert.ToInt32(see);
            Random rnd = new Random(seedInt);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                sb.Append(charArray[rnd.Next(60)]);
            }
            return sb.ToString();
        }

        #endregion

        private byte findCharacter(char c)
        {
            byte index = 0;
            bool found = false;
            for (byte i = 0; i < charArray.Length; i++)
            {
                if (c.Equals(charArray[i]))
                {
                    index = i;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                throw new Exception("Tanımlanmamış Karakter: " + c.ToString());
            }
            return index;

        }


        #region IScrambler Members

        public string createDecoder(string finalHtml)
        {
            string resultHtml = "<script language=\"JavaScript\" type=\"text/javascript\">"
                + MainPart(finalHtml) + charArrStr + strinArrStr;
           
            Dictionary<int, bool> isProcessedDic = new Dictionary<int, bool>();
            //butun dic değerleri stringe atılyor.
            while (isProcessedDic.Count<9)
            {
                int seed = Convert.ToInt32(DateTime.Now.Ticks % 100000);
                Random r = new Random(seed);
                int swap1 = r.Next(0, 9);
                if (!isProcessedDic.ContainsKey(swap1))
                {
                    resultHtml += methodDic[swap1].ToString();
                    isProcessedDic.Add(swap1, true);
                }            
            }        

            resultHtml += "</script>";
            return resultHtml;
        }       

        private string MainPart(string finalHtml)
        {
            StringBuilder js = new StringBuilder();
            Random rnd = new Random();
            //js.Append("<script language=\"JavaScript\" type=\"text/javascript\">");
            js.Append("var " + contentStringName + "=\"");
            js.Append(finalHtml);
            js.Append("\";");           
            return js.ToString();
        }

        private string GetR0Method()
        {

            StringBuilder js = new StringBuilder();
            Random rnd = new Random(DateTime.Now.Millisecond * DateTime.Now.Second);
            findStrMethodName = GRS(rnd.Next(3, 15)) + "" + rnd.Next(11, 17);
            decodeMethodName = GRS(rnd.Next(2, 11)) + "" + rnd.Next(4, 11);

            string decodedHTMLStringName = GRS(rnd.Next(1, 9)) + "" + rnd.Next(3);
            string HTMLStringParameterName = GRS(rnd.Next(1, 9)) + "" + rnd.Next(17, 33);
            string decNumStringName = HTMLStringParameterName + GRS(rnd.Next(1, 4));
            string realIndexString = GRS(rnd.Next(3, 9)) + "" + rnd.Next(39, 45) + "" + GRS(rnd.Next(2, 4));
            string sizeKeyForLoopindexD = GRS(rnd.Next(3, 10));
            string sizeKeyForLoopindexO = GRS(rnd.Next(3, 9));

            // string realFunc = "function " + decodeMethodName + "(" + HTMLStringParameterName + "){";
            string realFunc = " var i=0;var " + decNumStringName + "=(" + contentStringName + ".length)/" + (3 + sizeKey) + ";";
            realFunc += " var x=0;var " + decodedHTMLStringName + "='';var y=2;";
            realFunc += " var " + indexString + "=0;var " + realIndexString + "=0;";
            if (DateTime.Now.Millisecond > 500)
            {
                realFunc += " var " + GRS(rnd.Next(3, 4)) + "=''; var function_='function  " + GRS(rnd.Next(3, 5)) + "(" + contentStringName + ")";//dumm
                realFunc += "{";//dummy
                realFunc += " for (" + sizeKeyForLoopindexD + " = 0; " + sizeKeyForLoopindexD + " < " + decNumStringName + "; " + sizeKeyForLoopindexD + "++){";//dumm
                realFunc += " x=" + (3 + sizeKey) + "*" + sizeKeyForLoopindexD + ";";//dumm
                realFunc += " }";//dumm oof For
                realFunc += "}';";//dumm eof
            }

            realFunc += "for (" + sizeKeyForLoopindexO + " = 0; " + sizeKeyForLoopindexO + " < " + decNumStringName + "; " + sizeKeyForLoopindexO + "++){";
            realFunc += "x=" + (3 + sizeKey) + "*" + sizeKeyForLoopindexO + ";";
            realFunc += " subs=" + contentStringName + ".substr(x,3);";

            //+ "document.write(" + dumFuncN + "(" + decNumStringName + "));"
            if (DateTime.Now.Second > 40)
            {
                string varName = GRS(rnd.Next(3, 18)) + GRS(rnd.Next(4, 18));
                string j = GRS(rnd.Next(3, 5)) + GRS(rnd.Next(2, 4));
                realFunc += " var " + varName + " =' document.write(" + decodeMethodName + "(" + contentStringName + "))';";
                realFunc += " for (" + j + " = 0; " + j + " < " + sizeKey + "; " + j + "++){";//dumm
                realFunc += " y=" + (3 + sizeKey) + "*" + j + ";";//dumm
                realFunc += " }";//dumm oof For
            }

            realFunc += indexString + " = " + findStrMethodName + "(subs);";
            realFunc += realIndexString + "=" + indexString + "-" + stepKey + ";";
            realFunc += "if(" + realIndexString + "<0){ ";
            realFunc += realIndexString + "+=" + charArrayName + ".length;}";
            realFunc += decodedHTMLStringName + "+=" + charArrayName + "[" + realIndexString + "];";
            realFunc += "}";
            realFunc += "return " + decodedHTMLStringName + ";";
            // realFunc += "}";
            js.Append(realFunc);

            //fake --js.Append("document.write(" + dumFuncN + "(" + contentStringName + "));");
            return js.ToString();

        }
        //bazen çakan bazen iş yapmayan
        private string GetR1Method()
        {

            StringBuilder js = new StringBuilder();
            Random rnd = new Random();

            string findStrMethodName = GRS(rnd.Next(3, 9)) + "" + rnd.Next(11, 17);
            string decodeMethodName = GRS(rnd.Next(4, 11)) + "" + rnd.Next(3, 9);
            string indexString = GRS(rnd.Next(1, 3)) + "" + rnd.Next(1, 89) + "" + GRS(rnd.Next(1, 3));

            /**HELPER REAL FUNCT*/
            //js.Append("function " + findStrMethodName + "(c){var " + indexString + " = 0;for(i = 0; i < " + stringArrayName + ".length; i++){if (c==" + stringArrayName + "[i]){" + indexString + " = i; break;}} return " + indexString + ";}");

            string decodedHTMLStringName = GRS(rnd.Next(3, 13)) + "" + rnd.Next(3);
            string HTMLStringParameterName = GRS(rnd.Next(4, 15)) + "" + rnd.Next(17, 33);
            string decNumStringName = HTMLStringParameterName + GRS(rnd.Next(1, 4));
            string realIndexString = GRS(rnd.Next(7, 19)) + "" + rnd.Next(39, 45) + "" + GRS(rnd.Next(2, 9));
            string sizeKeyForLoopindexD = GRS(rnd.Next(7, 16));
            string sizeKeyForLoopindexO = GRS(rnd.Next(8, 19));

            /**MAIN REAL FUNCT F1*/
            string realFunc = "";
            //string realFunc = " function " + decodeMethodName + "(" + HTMLStringParameterName + "){";
            realFunc += " var i=0;var " + decNumStringName + "=(" + contentStringName + ".length)/" + (3 + sizeKey) + ";";
            realFunc += " var x=0;var " + decodedHTMLStringName + "='';var y=2;";
            realFunc += " var " + indexString + "=0;var " + realIndexString + "=0;";
            if (DateTime.Now.Millisecond%10 > 4)
            {
                realFunc += " var " + GRS(rnd.Next(4, 9)) + " =''; var function_='function  " + GRS(rnd.Next(3, 7)) + "(" + contentStringName + ")";//dumm
                realFunc += "{";
                realFunc += " for (" + sizeKeyForLoopindexD + " = 0; " + sizeKeyForLoopindexD + " < " + decNumStringName + "; " + sizeKeyForLoopindexD + "++){";//dumm
                realFunc += " x=" + (3 + sizeKey) + "*" + sizeKeyForLoopindexD + ";";//dumm
                realFunc += " }";//dumm oof For
                realFunc += "}';";//dumm eof
            }

            realFunc += "for (" + sizeKeyForLoopindexO + " = 0; " + sizeKeyForLoopindexO + " < " + decNumStringName + "; " + sizeKeyForLoopindexO + "++){";
            realFunc += "x=" + (3 + sizeKey) + "*" + sizeKeyForLoopindexO + ";";
            realFunc += "subs=" + contentStringName + ".substr(x,3);";

            //infinite loop
            if (DateTime.Now.Second < 30)
            {
                string varName = GRS(rnd.Next(3, 18)) + GRS(rnd.Next(4, 18));//dumm
                string j = GRS(rnd.Next(3, 5)) + GRS(rnd.Next(2, 4));//dumm
                realFunc += " var " + varName + " =' document.write(" + decodeMethodName + "(" + contentStringName + "))';";//dumm
                realFunc += " for (" + j + " = 0; " + j + " < " + sizeKey + "; " + j + "++){";//dumm
                realFunc += " y=" + (3 + sizeKey) + "*" + j + ";";//dumm
                realFunc += j + "= " + j + " - 1;";//dumm
                realFunc += " }";//dumm oof For
            }
            realFunc += indexString + " = " + findStrMethodName + "(subs);";
            realFunc += realIndexString + "=" + indexString + "-" + stepKey + ";";
            realFunc += " if(" + realIndexString + "<0){";
            realFunc += realIndexString + "+=" + charArrayName + ".length;}";
            realFunc += decodedHTMLStringName + "+=" + charArrayName + "[" + realIndexString + "];";
            realFunc += "}";
            realFunc += "return " + decodedHTMLStringName + ";";
            return realFunc;

        }
        //
        private string GetR2Method()
        {
            StringBuilder js = new StringBuilder();
            Random rnd = new Random();

            string findStrMethodName = GRS(rnd.Next(3, 9)) + "" + rnd.Next(11, 17);
            string decodeMethodName = GRS(rnd.Next(4, 11)) + "" + rnd.Next(3, 9);
            string indexString = GRS(rnd.Next(1, 3)) + "" + rnd.Next(1, 89) + "" + GRS(rnd.Next(1, 3));

            string decodedHTMLStringName = GRS(rnd.Next(3, 13)) + "" + rnd.Next(3);
            string HTMLStringParameterName = GRS(rnd.Next(4, 15)) + "" + rnd.Next(17, 33);
            string decNumStringName = HTMLStringParameterName + GRS(rnd.Next(1, 4));
            string realIndexString = GRS(rnd.Next(7, 19)) + "" + rnd.Next(39, 45) + "" + GRS(rnd.Next(2, 9));
            string sizeKeyForLoopindexD = GRS(rnd.Next(7, 16));
            string sizeKeyForLoopindexO = GRS(rnd.Next(8, 19));

            /**MAIN REAL FUNCT F2*/
            string realFunc = "";
            // string realFunc = " function " + decodeMethodName + "(" + HTMLStringParameterName + "){";
            realFunc += " var i=0;var " + decNumStringName + "=(" + contentStringName + ".length)/" + (3 + sizeKey) + ";";
            realFunc += " var x=0;var " + decodedHTMLStringName + "='';var y=2;";
            realFunc += " var " + indexString + "=0;var " + realIndexString + "=0;";
            realFunc += " var " + GRS(rnd.Next(4, 9)) + " =''; ";


            if (DateTime.Now.Second > 30)
            {
                string varName = GRS(rnd.Next(3, 18)) + GRS(rnd.Next(4, 18));
                string j = GRS(rnd.Next(3, 5)) + GRS(rnd.Next(2, 4));
                realFunc += " var " + varName + " =' document.write(" + decodeMethodName + "(" + contentStringName + "))';";
                realFunc += " for (" + j + " = 0; " + j + " < " + sizeKey + "; " + j + "++){";//dumm
                realFunc += " y=" + (3 + sizeKey) + "*" + j + ";";//dumm
                realFunc += " }";//dumm oof For          
            }


            realFunc += " for (" + sizeKeyForLoopindexD + " = 0; " + sizeKeyForLoopindexD + " < " + decNumStringName + "; " + sizeKeyForLoopindexD + "++){";//dumm
            realFunc += " x=" + (3 + sizeKey) + "*" + sizeKeyForLoopindexD + ";";//dumm
            realFunc += " }";//dumm oof For 

            if (DateTime.Now.Millisecond < 500)
            {
                realFunc += " var function_='function  " + GRS(rnd.Next(3, 7)) + "(" + contentStringName + ")";//dumm
                realFunc += "{";
                realFunc += " for (" + sizeKeyForLoopindexD + " = 0; " + sizeKeyForLoopindexD + " < " + decNumStringName + "; " + sizeKeyForLoopindexD + "++){";//dumm
                realFunc += " x=" + (3 + sizeKey) + "*" + sizeKeyForLoopindexD + ";";//dumm
                if(DateTime.Now.Second%2==1)
                    realFunc+=sizeKeyForLoopindexD+"=  "+sizeKeyForLoopindexD+" -  1";
                realFunc += " }";//dumm oof For
                realFunc += "}';";//dumm eof
            }
            //+ "document.write(" + dumFuncN + "(" + decNumStringName + "));"

            realFunc += "for (" + sizeKeyForLoopindexO + " = 0; " + sizeKeyForLoopindexO + " < " + decNumStringName + "; " + sizeKeyForLoopindexO + "++){";
            realFunc += "x=" + (3 + sizeKey) + "*" + sizeKeyForLoopindexO + ";";
            realFunc += "subs=" + HTMLStringParameterName + ".substr(x,3);";

            realFunc += indexString + " = " + findStrMethodName + "(subs);";
            realFunc += realIndexString + "=" + indexString + "-" + stepKey + ";";
            realFunc += "if(" + realIndexString + "<0){";
            realFunc += realIndexString + "+=" + charArrayName + ".length;}";
            realFunc += decodedHTMLStringName + "+=" + charArrayName + "[" + realIndexString + "];";
            realFunc += "}";
            realFunc += "return " + decodedHTMLStringName + ";";
            // realFunc += "}";    
            // ----fake
            // js.Append("document.write(" + dumFuncN + "(" + contentStringName + "));");
            return realFunc;

        }


        private string GetR3Method()
        {
            StringBuilder js = new StringBuilder();
            Random rnd = new Random();

            string findStrMethodName = GRS(rnd.Next(3, 9)) + "" + rnd.Next(11, 17);
            string decodeMethodName = GRS(rnd.Next(4, 11)) + "" + rnd.Next(3, 9);
            string indexString = GRS(rnd.Next(1, 3)) + "" + rnd.Next(1, 89) + "" + GRS(rnd.Next(1, 3));

            string decodedHTMLStringName = GRS(rnd.Next(3, 13)) + "" + rnd.Next(3);
            string HTMLStringParameterName = GRS(rnd.Next(4, 15)) + "" + rnd.Next(17, 33);
            string decNumStringName = HTMLStringParameterName + GRS(rnd.Next(1, 4));
            string realIndexString = GRS(rnd.Next(7, 19)) + "" + rnd.Next(39, 45) + "" + GRS(rnd.Next(2, 9));
            string sizeKeyForLoopindexD = GRS(rnd.Next(7, 16));
            string sizeKeyForLoopindexO = GRS(rnd.Next(8, 19));

            /**MAIN REAL FUNCT F2*/
            string realFunc = "";
            // string realFunc = " function " + decodeMethodName + "(" + HTMLStringParameterName + "){";
            realFunc += " var i=0;var " + decNumStringName + "=(" + contentStringName + ".length)/" + (3 + sizeKey) + ";";
            realFunc += " var x=0;var " + decodedHTMLStringName + "='';var y=2;";
            realFunc += " var " + indexString + "=0;var " + realIndexString + "=0;";
            realFunc += " var " + GRS(rnd.Next(4, 9)) + " =''; ";


            if (DateTime.Now.Minute%2 == 1 )
            {
                string varName = GRS(rnd.Next(3, 18)) + GRS(rnd.Next(4, 18));
                string j = GRS(rnd.Next(3, 5)) + GRS(rnd.Next(2, 4));
                realFunc += " var " + varName + " =' document.write(" + methodDic[0].MethodName + "(" + decNumStringName + "))';";
                realFunc += " for (" + j + " = 0; " + j + " < " + sizeKey + "; " + j + "++){";//dumm
                realFunc += " y=" + (3 + sizeKey) + "*" + j + ";";//dumm
                
                realFunc += " }";//dumm oof For          
            }


            realFunc += " for (" + sizeKeyForLoopindexD + " = 0; " + sizeKeyForLoopindexD + " < " + decNumStringName + "; " + sizeKeyForLoopindexD + "++){";//dumm
            realFunc += " x=" + (3 + sizeKey) + "*" + sizeKeyForLoopindexD + ";";//dumm
            realFunc += " }";//dumm oof For 

            if (DateTime.Now.Millisecond < 500)
            {
                realFunc += " var function_='function  " + GRS(rnd.Next(3, 7)) + "(" + contentStringName + ")";//dumm
                realFunc += "{";
                realFunc += " for (" + sizeKeyForLoopindexD + " = 0; " + sizeKeyForLoopindexD + " < " + decNumStringName + "; " + sizeKeyForLoopindexD + "++){";//dumm
                realFunc += " x=" + (3 + sizeKey) + "*" + sizeKeyForLoopindexD + ";";//dumm
                if (DateTime.Now.Second % 2 == 1)
                    realFunc += sizeKeyForLoopindexD + "=  " + sizeKeyForLoopindexD + " -  1";
                realFunc += " }";//dumm oof For
                realFunc += "}';";//dumm eof
            }
            //+ "document.write(" + dumFuncN + "(" + decNumStringName + "));"

            realFunc += "for (" + sizeKeyForLoopindexO + " = 0; " + sizeKeyForLoopindexO + " < " + decNumStringName + "; " + sizeKeyForLoopindexO + "++){";
            realFunc += "x=" + (3 + sizeKey) + "*" + sizeKeyForLoopindexO + ";";
            realFunc += "subs=" + HTMLStringParameterName + ".substr(x,3);";

            realFunc += indexString + " = " + findStrMethodName + "(subs);";
            realFunc += realIndexString + "=" + indexString + "-" + stepKey + ";";
            realFunc += "if(" + realIndexString + "<0){";
            realFunc += realIndexString + "+=" + charArrayName + ".length;}";
            realFunc += decodedHTMLStringName + "+=" + charArrayName + "[" + realIndexString + "];";
            realFunc += "}";
            realFunc += "return " + decodedHTMLStringName + ";";
            // realFunc += "}";    
            // ----fake
            // js.Append("document.write(" + dumFuncN + "(" + contentStringName + "));");
            return realFunc;

        }



        private string GetDummyMethod()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            //string indexString = GRS(rnd.Next(1, 8)) + "" + rnd.Next(1, 89) + "" + GRS(rnd.Next(1, 8));

            string dumFuncN = GRS(rnd.Next(4, 11)) + "" + rnd.Next(3, 9);
            string dummyVarName = GRS(rnd.Next(4, 7));
            string strMB = "function " + dumFuncN + "(" + contentStringName + "){"
                + " var " + dummyVarName + "='" + GRS(rnd.Next(1, 3))
                + "'; var var_function='function var function(var,function){document.write(var,function)}';"
                + " document.write(''); return "
                + dummyVarName + ";  }";
            return strMB;
        }

        private string DecodeMethod()
        {
            Random rnd = new Random(DateTime.Now.Millisecond * DateTime.Now.Second);
            string paramChar = GRS(rnd.Next(5, 10)) + GRS(rnd.Next(2, 7));
            string loopIndex = GRS(rnd.Next(2, 4)) + GRS(rnd.Next(3, 5));
            string decStr = "function " + findStrMethodName + "(" + paramChar + "){"
              + "var " + indexString + " = 0;"
              + "for(" + loopIndex + " = 0; " + loopIndex + " < " + stringArrayName + ".length; " + loopIndex + "++){"
              + "if (" + paramChar + "==" + stringArrayName + " [" + loopIndex + "]){"
              + indexString + " = " + loopIndex + "; break;} "
              + "}"
              + " return " + indexString + ";}";
            return decStr;
        }

        private string DecodeMethod2()
        {

            Random rnd = new Random(DateTime.Now.Millisecond * DateTime.Now.Second);
            string paramChar = GRS(rnd.Next(5, 10)) + GRS(rnd.Next(2, 7));
            string loopIndex = GRS(rnd.Next(2, 4)) + GRS(rnd.Next(3, 5));
            string dVar = GRS(rnd.Next(5, 15));//dummy
            string decStr = "function " + findStrMethodName + "(" + paramChar + "){"
              + "var " + indexString + " = 0;"
              + "for(" + loopIndex + " = 0; " + loopIndex + " < " + stringArrayName + ".length; " + loopIndex + "++){"
              + dVar + stringArrayName + "[i];"
              + "if (" + paramChar + "==" + stringArrayName + " [" + loopIndex + "]){"
              + indexString + " = " + loopIndex + "; break;} "
              + "}"
              + " return " + indexString + ";}";
            return decStr;

        }

        #endregion
    }
}
