//Abre uma janela com o url, nome, width, e height especificados.
//A janela é posicionada no centro da tela.
function openPopupWindow(url, nomeJanela, width, height) {
    centerX = (screen.width - width) / 2;
    centerY = (screen.height - height) / 2;
    window.open(url, nomeJanela, "resizable=1,width=" + width + ",height=" + height + ",top=" + centerY + ",left=" + centerX + ",location=0,toolbar=0,menubar=0,scrollbars=1");
}

function ConfirmarGravar() {
    return (window.confirm("Confirma a Gravação dos Dados ?"))
}


function ConfirmaInclusao() {
    return confirm('Deseja realmente incluir este registro?');
}

function ConfirmaAlteracao() {
    return confirm('Deseja realmente alterar este registro?');
}



function CallPrint(strid) {
    var prtContent = document.getElementById(strid);
    var WinPrint = window.open('', '', 'letf=0,top=0,width=600,height=400,toolbar=0,scrollbars=0,status=0');
    WinPrint.document.write(prtContent.innerHTML);
    WinPrint.document.close();
    WinPrint.focus();
    WinPrint.print();
    WinPrint.close();
    prtContent.innerHTML = strOldOne;
}

function colorTween(c1, c2, p) {
    var r1 = parseInt(c1.substring(1, 3), 16);
    var g1 = parseInt(c1.substring(3, 5), 16);
    var b1 = parseInt(c1.substring(5, 7), 16);
    var r2 = parseInt(c2.substring(1, 3), 16);
    var g2 = parseInt(c2.substring(3, 5), 16);
    var b2 = parseInt(c2.substring(5, 7), 16);
    var r3 = (256 + (r2 - r1) * p / 100 + r1).toString(16);
    var g3 = (256 + (g2 - g1) * p / 100 + g1).toString(16);
    var b3 = (256 + (b2 - b1) * p / 100 + b1).toString(16);
    return '#' + r3.substring(1, 3) + g3.substring(1, 3) + b3.substring(1, 3);
}

function DoBlur(fld) {
    fld.className = 'normalfld';
}

function DoFocus(fld) {
    fld.className = 'focusfld';
}


function isCPFCNPJ() {
    if (isEmpty(document.frmIndex.txtCNPJCPF.Value))
    { return false; }


    var campo = document.frmIndex.txtCNPJCPF.Value
    var pType = 0
    var campo_filtrado = "", valor_1 = " ", valor_2 = " ", ch = "";
    var valido = false;

    for (i = 0; i < campo.length; i++) {
        ch = campo.substring(i, i + 1);
        if (ch >= "0" && ch <= "9") {
            campo_filtrado = campo_filtrado.toString() + ch.toString()
            valor_1 = valor_2;
            valor_2 = ch;
        }
        if ((valor_1 != " ") && (!valido)) valido = !(valor_1 == valor_2);
    }
    if (!valido) campo_filtrado = "12345678912";

    if (campo_filtrado.length < 11) {
        for (i = 1; i <= (11 - campo_filtrado.length); i++) { campo_filtrado = "0" + campo_filtrado; }
    }

    if (pType <= 1) {
        if ((campo_filtrado.substring(9, 11) == checkCPF(campo_filtrado.substring(0, 9))) && (campo_filtrado.substring(11, 12) == "")) { return true; }
    }

    if ((pType == 2) || (pType == 0)) {
        if (campo_filtrado.length >= 14) {
            if (campo_filtrado.substring(12, 14) == checkCNPJ(campo_filtrado.substring(0, 12))) { return true; }
        }
    }

    alert('Este número de CPF/CNPJ é Inválido');
    document.Formulario.txtCNPJCPF.value = '';
    document.Formulario.txtCNPJCPF.focus();
    return false;
}

function checkCNPJ(vCNPJ) {
    var mControle = "";
    var aTabCNPJ = new Array(5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2);
    for (i = 1; i <= 2; i++) {
        mSoma = 0;
        for (j = 0; j < vCNPJ.length; j++)
            mSoma = mSoma + (vCNPJ.substring(j, j + 1) * aTabCNPJ[j]);
        if (i == 2) mSoma = mSoma + (2 * mDigito);
        mDigito = (mSoma * 10) % 11;
        if (mDigito == 10) mDigito = 0;
        mControle1 = mControle;
        mControle = mDigito;
        aTabCNPJ = new Array(6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3);
    }
    return ((mControle1 * 10) + mControle);
}

function checkCPF(vCPF) {
    var mControle = ""
    var mContIni = 2, mContFim = 10, mDigito = 0;
    for (j = 1; j <= 2; j++) {
        mSoma = 0;
        for (i = mContIni; i <= mContFim; i++)
            mSoma = mSoma + (vCPF.substring((i - j - 1), (i - j)) * (mContFim + 1 + j - i));
        if (j == 2) mSoma = mSoma + (2 * mDigito);
        mDigito = (mSoma * 10) % 11;
        if (mDigito == 10) mDigito = 0;
        mControle1 = mControle;
        mControle = mDigito;
        mContIni = 3;
        mContFim = 11;
    }
    return ((mControle1 * 10) + mControle);
}

// Bloco de Novas implementações

// Check browser version
var isNav4 = false, isNav5 = false, isIE4 = false
var strSeperator = "/";
// If you are using any Java validation on the back side you will want to use the / because 
// Java date validations do not recognize the dash as a valid date separator.
var vDateType = 3; // Global value for type of date format
//                1 = mm/dd/yyyy
//                2 = yyyy/dd/mm  (Unable to do date check at this time)
//                3 = dd/mm/yyyy
var vYearType = 4; //Set to 2 or 4 for number of digits in the year for Netscape
var vYearLength = 2; // Set to 4 if you want to force the user to enter 4 digits for the year before validating.
var err = 0; // Set the error code to a default of zero
if (navigator.appName == "Netscape") {
    if (navigator.appVersion < "5") {
        isNav4 = true;
        isNav5 = false;
    }
    else
        if (navigator.appVersion > "4") {
            isNav4 = false;
            isNav5 = true;
        }
}
else {
    isIE4 = true;
}
function DateFormat(vDateName, vDateValue, e, dateCheck, dateType) {
    vDateType = dateType;
    // vDateName = object name
    // vDateValue = value in the field being checked
    // e = event
    // dateCheck 
    // True  = Verify that the vDateValue is a valid date
    // False = Format values being entered into vDateValue only
    // vDateType
    // 1 = mm/dd/yyyy
    // 2 = yyyy/mm/dd
    // 3 = dd/mm/yyyy
    //Enter a tilde sign for the first number and you can check the variable information.
    if (vDateValue == "~") {
        alert("AppVersion = " + navigator.appVersion + " \nNav. 4 Version = " + isNav4 + " \nNav. 5 Version = " + isNav5 + " \nIE Version = " + isIE4 + " \nYear Type = " + vYearType + " \nDate Type = " + vDateType + " \nSeparator = " + strSeperator);
        //vDateName.value = "";
        vDateName.focus();
        return true;
    }
    var whichCode = (window.Event) ? e.which : e.keyCode;
    // Check to see if a seperator is already present.
    // bypass the date if a seperator is present and the length greater than 8
    if (vDateValue.length > 8 && isNav4) {
        if ((vDateValue.indexOf("-") >= 1) || (vDateValue.indexOf("/") >= 1))
            return true;
    }
    //Eliminate all the ASCII codes that are not valid
    var alphaCheck = " abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ/-";
    if (alphaCheck.indexOf(vDateValue) >= 1) {
        if (isNav4) {
            //vDateName.value = "";
            vDateName.focus();
            vDateName.select();
            return false;
        }
        else {
            //vDateName.value = vDateName.value.substr(0, (vDateValue.length-1));
            return false;
        }
    }
    if (whichCode == 8) //Ignore the Netscape value for backspace. IE has no value
        return false;
    else {
        //Create numeric string values for 0123456789/
        //The codes provided include both keyboard and keypad values
        var strCheck = '47,48,49,50,51,52,53,54,55,56,57,58,59,95,96,97,98,99,100,101,102,103,104,105';
        if (strCheck.indexOf(whichCode) != -1) {
            if (isNav4) {
                if (((vDateValue.length < 6 && dateCheck) || (vDateValue.length == 7 && dateCheck)) && (vDateValue.length >= 1)) {
                    alert("Invalid Date\nPlease Re-Enter");
                    //vDateName.value = "";
                    vDateName.focus();
                    vDateName.select();
                    return false;
                }
                if (vDateValue.length == 6 && dateCheck) {
                    var mDay = vDateName.value.substr(2, 2);
                    var mMonth = vDateName.value.substr(0, 2);
                    var mYear = vDateName.value.substr(4, 4)
                    //Turn a two digit year into a 4 digit year
                    if (mYear.length == 2 && vYearType == 4) {
                        var mToday = new Date();
                        //If the year is greater than 30 years from now use 19, otherwise use 20
                        var checkYear = mToday.getFullYear() + 30;
                        var mCheckYear = '20' + mYear;
                        if (mCheckYear >= checkYear)
                            mYear = '19' + mYear;
                        else
                            mYear = '20' + mYear;
                    }
                    var vDateValueCheck = mMonth + strSeperator + mDay + strSeperator + mYear;
                    if (!dateValid(vDateValueCheck)) {
                        alert("Invalid Date\nPlease Re-Enter");
                        //vDateName.value = "";
                        vDateName.focus();
                        vDateName.select();
                        return false;
                    }
                    return true;
                }
                else {
                    // Reformat the date for validation and set date type to a 1
                    if (vDateValue.length >= 8 && dateCheck) {
                        if (vDateType == 1) // mmddyyyy
                        {
                            var mDay = vDateName.value.substr(2, 2);
                            var mMonth = vDateName.value.substr(0, 2);
                            var mYear = vDateName.value.substr(4, 4)
                            vDateName.value = mMonth + strSeperator + mDay + strSeperator + mYear;
                        }
                        if (vDateType == 2) // yyyymmdd
                        {
                            var mYear = vDateName.value.substr(0, 4)
                            var mMonth = vDateName.value.substr(4, 2);
                            var mDay = vDateName.value.substr(6, 2);
                            vDateName.value = mYear + strSeperator + mMonth + strSeperator + mDay;
                        }
                        if (vDateType == 3) // ddmmyyyy
                        {
                            var mMonth = vDateName.value.substr(2, 2);
                            var mDay = vDateName.value.substr(0, 2);
                            var mYear = vDateName.value.substr(4, 4)
                            vDateName.value = mDay + strSeperator + mMonth + strSeperator + mYear;
                        }
                        //Create a temporary variable for storing the DateType and change
                        //the DateType to a 1 for validation.
                        var vDateTypeTemp = vDateType;
                        vDateType = 1;
                        var vDateValueCheck = mMonth + strSeperator + mDay + strSeperator + mYear;
                        if (!dateValid(vDateValueCheck)) {
                            alert("Invalid Date\nPlease Re-Enter");
                            vDateType = vDateTypeTemp;
                            //vDateName.value = "";
                            vDateName.focus();
                            vDateName.select();
                            return false;
                        }
                        vDateType = vDateTypeTemp;
                        return true;
                    }
                    else {
                        if (((vDateValue.length < 8 && dateCheck) || (vDateValue.length == 9 && dateCheck)) && (vDateValue.length >= 1)) {
                            alert("Invalid Date\nPlease Re-Enter");
                            //vDateName.value = "";
                            vDateName.focus();
                            vDateName.select();
                            return false;
                        }
                    }
                }
            }
            else {
                // Non isNav Check
                if (((vDateValue.length < 8 && dateCheck) || (vDateValue.length == 9 && dateCheck)) && (vDateValue.length >= 1)) {
                    alert("Invalid Date\nPlease Re-Enter");
                    //vDateName.value = "";
                    vDateName.focus();
                    return true;
                }
                // Reformat date to format that can be validated. mm/dd/yyyy
                if (vDateValue.length >= 8 && dateCheck) {
                    // Additional date formats can be entered here and parsed out to
                    // a valid date format that the validation routine will recognize.
                    if (vDateType == 1) // mm/dd/yyyy
                    {
                        var mMonth = vDateName.value.substr(0, 2);
                        var mDay = vDateName.value.substr(3, 2);
                        var mYear = vDateName.value.substr(6, 4)
                    }
                    if (vDateType == 2) // yyyy/mm/dd
                    {
                        var mYear = vDateName.value.substr(0, 4)
                        var mMonth = vDateName.value.substr(5, 2);
                        var mDay = vDateName.value.substr(8, 2);
                    }
                    if (vDateType == 3) // dd/mm/yyyy
                    {
                        var mDay = vDateName.value.substr(0, 2);
                        var mMonth = vDateName.value.substr(3, 2);
                        var mYear = vDateName.value.substr(6, 4)
                    }
                    if (vYearLength == 4) {
                        if (mYear.length < 4) {
                            alert("Invalid Date\nPlease Re-Enter");
                            //vDateName.value = "";
                            vDateName.focus();
                            return true;
                        }
                    }
                    // Create temp. variable for storing the current vDateType
                    var vDateTypeTemp = vDateType;
                    // Change vDateType to a 1 for standard date format for validation
                    // Type will be changed back when validation is completed.
                    vDateType = 1;
                    // Store reformatted date to new variable for validation.
                    var vDateValueCheck = mMonth + strSeperator + mDay + strSeperator + mYear;
                    if (mYear.length == 2 && vYearType == 4 && dateCheck) {
                        //Turn a two digit year into a 4 digit year
                        var mToday = new Date();
                        //If the year is greater than 30 years from now use 19, otherwise use 20
                        var checkYear = mToday.getFullYear() + 30;
                        var mCheckYear = '20' + mYear;
                        if (mCheckYear >= checkYear)
                            mYear = '19' + mYear;
                        else
                            mYear = '20' + mYear;
                        vDateValueCheck = mMonth + strSeperator + mDay + strSeperator + mYear;
                        // Store the new value back to the field.  This function will
                        // not work with date type of 2 since the year is entered first.
                        if (vDateTypeTemp == 1) // mm/dd/yyyy
                            vDateName.value = mMonth + strSeperator + mDay + strSeperator + mYear;
                        if (vDateTypeTemp == 3) // dd/mm/yyyy
                            vDateName.value = mDay + strSeperator + mMonth + strSeperator + mYear;
                    }
                    if (!dateValid(vDateValueCheck)) {
                        alert("Invalid Date\nPlease Re-Enter");
                        vDateType = vDateTypeTemp;
                        //vDateName.value = "";
                        vDateName.focus();
                        return true;
                    }
                    vDateType = vDateTypeTemp;
                    return true;
                }
                else {
                    if (vDateType == 1) {
                        if (vDateValue.length == 2) {
                            vDateName.value = vDateValue + strSeperator;
                        }
                        if (vDateValue.length == 5) {
                            vDateName.value = vDateValue + strSeperator;
                        }
                    }
                    if (vDateType == 2) {
                        if (vDateValue.length == 4) {
                            vDateName.value = vDateValue + strSeperator;
                        }
                        if (vDateValue.length == 7) {
                            vDateName.value = vDateValue + strSeperator;
                        }
                    }
                    if (vDateType == 3) {
                        if (vDateValue.length == 2) {
                            vDateName.value = vDateValue + strSeperator;
                        }
                        if (vDateValue.length == 5) {
                            vDateName.value = vDateValue + strSeperator;
                        }
                    }
                    return true;
                }
            }
            if (vDateValue.length == 10 && dateCheck) {
                if (!dateValid(vDateName)) {
                    // Un-comment the next line of code for debugging the dateValid() function error messages
                    //alert(err);  
                    alert("Invalid Date\nPlease Re-Enter");
                    vDateName.focus();
                    vDateName.select();
                }
            }
            return false;
        }
        else {
            // If the value is not in the string return the string minus the last
            // key entered.
            if (isNav4) {
                //vDateName.value = "";
                vDateName.focus();
                vDateName.select();
                return false;
            }
            else {
                //vDateName.value = vDateName.value.substr(0, (vDateValue.length-1));
                return false;
            }
        }
    }
}
function dateValid(objName) {
    var strDate;
    var strDateArray;
    var strDay;
    var strMonth;
    var strYear;
    var intday;
    var intMonth;
    var intYear;
    var booFound = false;
    var datefield = objName;
    var strSeparatorArray = new Array("-", " ", "/", ".");
    var intElementNr;
    // var err = 0;
    var strMonthArray = new Array(12);
    strMonthArray[0] = "Jan";
    strMonthArray[1] = "Feb";
    strMonthArray[2] = "Mar";
    strMonthArray[3] = "Apr";
    strMonthArray[4] = "May";
    strMonthArray[5] = "Jun";
    strMonthArray[6] = "Jul";
    strMonthArray[7] = "Aug";
    strMonthArray[8] = "Sep";
    strMonthArray[9] = "Oct";
    strMonthArray[10] = "Nov";
    strMonthArray[11] = "Dec";
    //strDate = datefield.value;
    strDate = objName;
    if (strDate.length < 1) {
        return true;
    }
    for (intElementNr = 0; intElementNr < strSeparatorArray.length; intElementNr++) {
        if (strDate.indexOf(strSeparatorArray[intElementNr]) != -1) {
            strDateArray = strDate.split(strSeparatorArray[intElementNr]);
            if (strDateArray.length != 3) {
                err = 1;
                return false;
            }
            else {
                strDay = strDateArray[0];
                strMonth = strDateArray[1];
                strYear = strDateArray[2];
            }
            booFound = true;
        }
    }
    if (booFound == false) {
        if (strDate.length > 5) {
            strDay = strDate.substr(0, 2);
            strMonth = strDate.substr(2, 2);
            strYear = strDate.substr(4);
        }
    }
    //Adjustment for short years entered
    if (strYear.length == 2) {
        strYear = '20' + strYear;
    }
    strTemp = strDay;
    strDay = strMonth;
    strMonth = strTemp;
    intday = parseInt(strDay, 10);
    if (isNaN(intday)) {
        err = 2;
        return false;
    }
    intMonth = parseInt(strMonth, 10);
    if (isNaN(intMonth)) {
        for (i = 0; i < 12; i++) {
            if (strMonth.toUpperCase() == strMonthArray[i].toUpperCase()) {
                intMonth = i + 1;
                strMonth = strMonthArray[i];
                i = 12;
            }
        }
        if (isNaN(intMonth)) {
            err = 3;
            return false;
        }
    }
    intYear = parseInt(strYear, 10);
    if (isNaN(intYear)) {
        err = 4;
        return false;
    }
    if (intMonth > 12 || intMonth < 1) {
        err = 5;
        return false;
    }
    if ((intMonth == 1 || intMonth == 3 || intMonth == 5 || intMonth == 7 || intMonth == 8 || intMonth == 10 || intMonth == 12) && (intday > 31 || intday < 1)) {
        err = 6;
        return false;
    }
    if ((intMonth == 4 || intMonth == 6 || intMonth == 9 || intMonth == 11) && (intday > 30 || intday < 1)) {
        err = 7;
        return false;
    }
    if (intMonth == 2) {
        if (intday < 1) {
            err = 8;
            return false;
        }
        if (LeapYear(intYear) == true) {
            if (intday > 29) {
                err = 9;
                return false;
            }
        }
        else {
            if (intday > 28) {
                err = 10;
                return false;
            }
        }
    }
    return true;
}
function LeapYear(intYear) {
    if (intYear % 100 == 0) {
        if (intYear % 400 == 0) { return true; }
    }
    else {
        if ((intYear % 4) == 0) { return true; }
    }
    return false;
}



//Funcao para validacao de Cnpj
function ValidaCnpj(source, args) {
    var cnpj = args.Value;
    var i, j, soma, cnpjaux;
    var dig1, dig2;

    args.IsValid = true;
    return;


    if (cnpj == "00000000000000") {
        args.IsValid = true;
        return;
    }


    var len = cnpj.length;

    //Valida argumento


    if ((len != 14) || (len != 11) || (isNaN(cnpj))) {
        args.IsValid = false;
        return;
    }


    if ((len == 14)) {


        if ((len != 14) || (isNaN(cnpj))) {
            args.IsValid = false;
            return;
        }


        for (var i = 0; i < 12; i++) {
            j = cnpj.substring(i, i + 1);
            eval("cnpj" + (i + 1) + "=" + j);
        }

        soma = (cnpj1 * 5) + (cnpj2 * 4) + (cnpj3 * 3) + (cnpj4 * 2) + (cnpj5 * 9) + (cnpj6 * 8) +
           (cnpj7 * 7) + (cnpj8 * 6) + (cnpj9 * 5) + (cnpj10 * 4) + (cnpj11 * 3) + (cnpj12 * 2);

        dig1 = soma - (Math.floor(soma / 11) * 11);

        if ((dig1 == 0) || (dig1 == 1))
            dig1 = 0;
        else
            dig1 = 11 - dig1;

        soma = (cnpj1 * 6) + (cnpj2 * 5) + (cnpj3 * 4) + (cnpj4 * 3) + (cnpj5 * 2) + (cnpj6 * 9) +
           (cnpj7 * 8) + (cnpj8 * 7) + (cnpj9 * 6) + (cnpj10 * 5) + (cnpj11 * 4) + (cnpj12 * 3) +
           (dig1 * 2);
        dig2 = soma - (Math.floor(soma / 11) * 11);

        if ((dig2 == 0) || (dig2 == 1))
            dig2 = 0;
        else
            dig2 = 11 - dig2;

        if ((dig1 + "" + dig2) == cnpj.substring(len, len - 2)) {
            args.IsValid = true;
        }
        else
            args.IsValid = false;

    }


    if ((len == 11)) {


        if ((len != 11) || (isNaN(cnpj))) {
            argS.IsValid = false;
            return;
        }

        soma = 0;
        for (i = 0, j = 10; i <= (len - 3); i += 1, j -= 1) {
            soma = soma + cnpj.substring(i, i + 1) * j;
        }
        dig1 = 11 - (soma % 11);
        if ((dig1 == 10) || (dig1 == 11)) {
            dig1 = 0;
        }

        cnpjaux = cnpj + dig1;
        soma = 0;
        for (i = 0, j = 11; i <= (len - 2); i += 1, j -= 1) {
            soma = soma + cnpjaux.substring(i, i + 1) * j;
        }
        dig2 = 11 - (soma % 11);
        if ((dig2 == 10) || (dig2 == 11)) {
            dig2 = 0;
        }

        if ((dig1 + "" + dig2) == cnpj.substring(len, len - 2)) {
            args.IsValid = true;
            return;
        }


        args.IsValid = false;
    }


    return;
}

function number_format(number, decimals, dec_point, thousands_sep) {
    // Formats a number with grouped thousands
    //
    // version: 906.1806
    // discuss at: http://phpjs.org/functions/number_format
    // +   original by: Jonas Raoni Soares Silva (http://www.jsfromhell.com)
    // +   improved by: Kevin van Zonneveld (http://kevin.vanzonneveld.net)
    // +     bugfix by: Michael White (http://getsprink.com)
    // +     bugfix by: Benjamin Lupton
    // +     bugfix by: Allan Jensen (http://www.winternet.no)
    // +    revised by: Jonas Raoni Soares Silva (http://www.jsfromhell.com)
    // +     bugfix by: Howard Yeend
    // +    revised by: Luke Smith (http://lucassmith.name)
    // +     bugfix by: Diogo Resende
    // +     bugfix by: Rival
    // +     input by: Kheang Hok Chin (http://www.distantia.ca/)
    // +     improved by: davook
    // +     improved by: Brett Zamir (http://brett-zamir.me)
    // +     input by: Jay Klehr
    // +     improved by: Brett Zamir (http://brett-zamir.me)
    // +     input by: Amir Habibi (http://www.residence-mixte.com/)
    // +     bugfix by: Brett Zamir (http://brett-zamir.me)
    // *     example 1: number_format(1234.56);
    // *     returns 1: '1,235'
    // *     example 2: number_format(1234.56, 2, ',', ' ');
    // *     returns 2: '1 234,56'
    // *     example 3: number_format(1234.5678, 2, '.', '');
    // *     returns 3: '1234.57'
    // *     example 4: number_format(67, 2, ',', '.');
    // *     returns 4: '67,00'
    // *     example 5: number_format(1000);
    // *     returns 5: '1,000'
    // *     example 6: number_format(67.311, 2);
    // *     returns 6: '67.31'
    // *     example 7: number_format(1000.55, 1);
    // *     returns 7: '1,000.6'
    // *     example 8: number_format(67000, 5, ',', '.');
    // *     returns 8: '67.000,00000'
    // *     example 9: number_format(0.9, 0);
    // *     returns 9: '1'
    // *     example 10: number_format('1.20', 2);
    // *     returns 10: '1.20'
    // *     example 11: number_format('1.20', 4);
    // *     returns 11: '1.2000'
    // *     example 12: number_format('1.2000', 3);
    // *     returns 12: '1.200'
    var n = number, prec = decimals;

    var toFixedFix = function (n, prec) {
        var k = Math.pow(10, prec);
        return (Math.round(n * k) / k).toString();
    };

    n = !isFinite(+n) ? 0 : +n;
    prec = !isFinite(+prec) ? 0 : Math.abs(prec);
    var sep = (typeof thousands_sep === 'undefined') ? ',' : thousands_sep;
    var dec = (typeof dec_point === 'undefined') ? '.' : dec_point;

    var s = (prec > 0) ? toFixedFix(n, prec) : toFixedFix(Math.round(n), prec); //fix for IE parseFloat(0.55).toFixed(0) = 0;

    var abs = toFixedFix(Math.abs(n), prec);
    var _, i;

    if (abs >= 1000) {
        _ = abs.split(/\D/);
        i = _[0].length % 3 || 3;

        _[0] = s.slice(0, i + (n < 0)) +
          _[0].slice(i).replace(/(\d{3})/g, sep + '$1');
        s = _.join(dec);
    } else {
        s = s.replace('.', dec);
    }

    var decPos = s.indexOf(dec);
    if (prec >= 1 && decPos !== -1 && (s.length - decPos - 1) < prec) {
        s += new Array(prec - (s.length - decPos - 1)).join(0) + '0';
    }
    else if (prec >= 1 && decPos === -1) {
        s += dec + new Array(prec).join(0) + '0';
    }
    return s;
}







