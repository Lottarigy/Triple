using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triple.LEXER
{
    public enum TokenType : byte
    {
        PLS,
        MIN,
        MUL,
        DIV,
        MOD,
        EXP,
        ASN,

        NML,
        SGL,
        CHL,
        BLL,

        NMD,
        SGD,
        CHD,
        BLD,
        STD,
        VDD,

        EQL,
        NEQ,
        GRT,
        LSS,
        GEQ,
        LEQ,

        LPN,
        RPN,
        LSQ,
        RSQ,
        LBC,
        RBC,
        CMA,
        DOT,
        VCT,
        ARW,
        CLN,
        DCL,
        FNC,
        SRC,
        VLN,

        LET,
        TID,
        TIF,
        ELS,
        ELI,
        WHL,
        TDO,
        RTN,
        STM,
        STR,
        BND,
        STC,

        OUT,
        OTL,
        INP,

        EOF
    }
}

