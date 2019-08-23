function convert_to_unicode() {
    var array_one = new Array("ç", "˜", ".", "'m", "]m", "Fmf", "Fm", ")", "!", "@", "#", "$", "%", "^", "&", "*", "(", "k|m", "em", "km", "Qm", "qm", "N˜", "¡", "¢", "1", "2", "4", ">", "?", "B", "I", "Q", "ß", "q", "„", "‹", "•", "›", "§", "°", "¶", "¿", "Å", "Ë", "Ì", "Í", "Î", "Ý", "å", "6«", "7«", "8«", "9«", "Ø", "|", "8Þ", "9Þ", "S", "s", "V", "v", "U", "u", "£", "3", "ª", "R", "r", "5", "H", "h", "‰", "´", "~", "`", "6", "7", "8", "9", "0", "T", "t", "Y", "y", "b", "W", "w", "G", "g", "K", "k", "ˆ", "A", "a", "E", "e", "D", "d", "o", "/", "N", "n", "J", "j", "Z", "z", "i", ":", ";", "X", "x", "cf‘", "c‘f", "cf}", "cf]", "cf", "c", "O{", "O", "pm", "p", "C", "P]", "P", "f‘", "\"", "'", "+", "f", "[", "\\", "]", "}", "F", "L", "M", "्ा", "्ो", "्ौ", "अो", "अा", "आै", "आे", "ाो", "ाॅ", "ाे", "ंु", "ेे", "अै", "ाे", "अे", "ंा", "अॅ", "ाै", "ैा", "ंृ", "ँा", "ँू", "ेा", "ंे")
    var array_two = new Array("ॐ", "ऽ", "।", "m'", "m]", "mfF", "mF", "०", "१", "२", "३", "४", "५", "६", "७", "८", "९", "फ्र", "झ", "फ", "क्त", "क्र", "ल", "ज्ञ्", "द्घ", "ज्ञ", "द्द", "द्ध", "श्र", "रु", "द्य", "क्ष्", "त्त", "द्म", "त्र", "ध्र", "ङ्घ", "ड्ड", "द्र", "ट्ट", "ड्ढ", "ठ्ठ", "रू", "हृ", "ङ्ग", "त्र", "ङ्क", "ङ्ख", "ट्ठ", "द्व", "ट्र", "ठ्र", "ड्र", "ढ्र", "्य", "्र", "ड़", "ढ़", "क्", "क", "ख्", "ख", "ग्", "ग", "घ्", "घ", "ङ", "च्", "च", "छ", "ज्", "ज", "झ्", "झ", "ञ्", "ञ", "ट", "ठ", "ड", "ढ", "ण्", "त्", "त", "थ्", "थ", "द", "ध्", "ध", "न्", "न", "प्", "प", "फ्", "ब्", "ब", "भ्", "भ", "म्", "म", "य", "र", "ल्", "ल", "व्", "व", "श्", "श", "ष्", "स्", "स", "ह्", "ह", "ऑ", "ऑ", "औ", "ओ", "आ", "अ", "ई", "इ", "ऊ", "उ", "ऋ", "ऐ", "ए", "ॉ", "ू", "ु", "ं", "ा", "ृ", "्", "े", "ै", "ँ", "ी", "ः", "", "े", "ै", "ओ", "आ", "औ", "ओ", "ो", "ॉ", "ो", "ुं", "े", "अ‍ै", "ो", "अ‍े", "ां", "अ‍ॅ", "ौ", "ौ", "ृं", "ाँ", "ूँ", "ो", "ें")
    var array_one_length = array_one.length;
    var body = document.getElementById("Report").innerHTML;

    var elements = document.getElementById("Report").querySelectorAll(".nepalifont");

    for (var i = 0; i < elements.length; i++) {
        var modified_substring = "";
        var processed_text = '';
     
     
        var modified_substring = elements[i].innerHTML;
            Replace_Symbols();
            processed_text += modified_substring;
        elements[i].innerHTML = processed_text;
        elements[i].style.fontSize = "small";
      
    }

    PrintElem("Report");
  

    function Replace_Symbols() {
        if (modified_substring != "") {
            for (input_symbol_idx = 0; input_symbol_idx < array_one_length; input_symbol_idx++) {
                idx = 0;
                while (idx != -1) {
                    modified_substring = modified_substring.replace(array_one[input_symbol_idx], array_two[input_symbol_idx])
                    idx = modified_substring.indexOf(array_one[input_symbol_idx])
                }
            }
            var position_of_i = modified_substring.indexOf("l")
            while (position_of_i != -1) {
                var charecter_next_to_i = modified_substring.charAt(position_of_i + 1)
                var charecter_to_be_replaced = "l" + charecter_next_to_i
                modified_substring = modified_substring.replace(charecter_to_be_replaced, charecter_next_to_i + "ि")
                position_of_i = modified_substring.search(/l/, position_of_i + 1)
            }
            var position_of_wrong_ee = modified_substring.indexOf("ि्")
            while (position_of_wrong_ee != -1) {
                var consonent_next_to_wrong_ee = modified_substring.charAt(position_of_wrong_ee + 2)
                var charecter_to_be_replaced = "ि्" + consonent_next_to_wrong_ee
                modified_substring = modified_substring.replace(charecter_to_be_replaced, "्" + consonent_next_to_wrong_ee + "ि")
                position_of_wrong_ee = modified_substring.search(/ि्/, position_of_wrong_ee + 2)
            }
            var position_of_wrong_ee = modified_substring.indexOf("िं्")
            while (position_of_wrong_ee != -1) {
                var consonent_next_to_wrong_ee = modified_substring.charAt(position_of_wrong_ee + 3)
                var charecter_to_be_replaced = "िं्" + consonent_next_to_wrong_ee
                modified_substring = modified_substring.replace(charecter_to_be_replaced, "्" + consonent_next_to_wrong_ee + "िं")
                position_of_wrong_ee = modified_substring.search(/िं्/, position_of_wrong_ee + 3)
            }
            set_of_matras = "ा ि ी ु ू ृ े ै ो ौ ं : ँ ॅ"
            var position_of_R = modified_substring.indexOf("{")
            while (position_of_R > 0) {
                probable_position_of_half_r = position_of_R - 1;
                var charecter_at_probable_position_of_half_r = modified_substring.charAt(probable_position_of_half_r)
                while (set_of_matras.match(charecter_at_probable_position_of_half_r) != null) {
                    probable_position_of_half_r = probable_position_of_half_r - 1;
                    charecter_at_probable_position_of_half_r = modified_substring.charAt(probable_position_of_half_r);
                }
                charecter_to_be_replaced = modified_substring.substr(probable_position_of_half_r, (position_of_R - probable_position_of_half_r));
                new_replacement_string = "र्" + charecter_to_be_replaced;
                charecter_to_be_replaced = charecter_to_be_replaced + "{";
                modified_substring = modified_substring.replace(charecter_to_be_replaced, new_replacement_string);
                position_of_R = modified_substring.indexOf("{");
            }
            modified_substring = modified_substring.replace(/=/g, ".");
            modified_substring = modified_substring.replace(/_/g, ")");
            modified_substring = modified_substring.replace(/Ö/g, "=");
            modified_substring = modified_substring.replace(/Ù/g, ";");
            modified_substring = modified_substring.replace(/…/g, "‘");
            modified_substring = modified_substring.replace(/Ú/g, "’");
            modified_substring = modified_substring.replace(/Û/g, "!");
            modified_substring = modified_substring.replace(/Ü/g, "%");
            modified_substring = modified_substring.replace(/æ/g, "“");
            modified_substring = modified_substring.replace(/Æ/g, "”");
            modified_substring = modified_substring.replace(/±/g, "+");
            modified_substring = modified_substring.replace(/-/g, "(");
            modified_substring = modified_substring.replace(/</g, "?");
        }
    }
}