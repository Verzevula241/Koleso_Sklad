using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;

/// <summary>
/// Класс для вызова функций API NetBIOS
/// (определение MAC-адресов и др.)
/// 
/// Разработано: Карпенко М.М.
/// </summary>
public class NetBIOS
{
    //API-функция NetBIOS
    [DllImport("netapi32.dll")]
    static extern byte Netbios(IntPtr pNCB);

    //команды NETBios
    public const byte NCBCALL = 0x10;           /* NCB CALL                           */
    public const byte NCBLISTEN = 0x11;         /* NCB LISTEN                         */
    public const byte NCBHANGUP = 0x12;         /* NCB HANG UP                        */
    public const byte NCBSEND = 0x14;           /* NCB SEND                           */
    public const byte NCBRECV = 0x15;           /* NCB RECEIVE                        */
    public const byte NCBRECVANY = 0x16;        /* NCB RECEIVE ANY                    */
    public const byte NCBCHAINSEND = 0x17;      /* NCB CHAIN SEND                     */
    public const byte NCBDGSEND = 0x20;         /* NCB SEND DATAGRAM                  */
    public const byte NCBDGRECV = 0x21;         /* NCB RECEIVE DATAGRAM               */
    public const byte NCBDGSENDBC = 0x22;       /* NCB SEND BROADCAST DATAGRAM        */
    public const byte NCBDGRECVBC = 0x23;       /* NCB RECEIVE BROADCAST DATAGRAM     */
    public const byte NCBADDNAME = 0x30;        /* NCB ADD NAME                       */
    public const byte NCBDELNAME = 0x31;        /* NCB DELETE NAME                    */
    public const byte NCBRESET = 0x32;          /* NCB RESET                          */
    public const byte NCBASTAT = 0x33;          /* NCB ADAPTER STATUS                 */
    public const byte NCBSSTAT = 0x34;          /* NCB SESSION STATUS                 */
    public const byte NCBCANCEL = 0x35;         /* NCB CANCEL                         */
    public const byte NCBADDGRNAME = 0x36;      /* NCB ADD GROUP NAME                 */
    public const byte NCBENUM = 0x37;           /* NCB ENUMERATE LANA NUMBERS         */
    public const byte NCBUNLINK = 0x70;         /* NCB UNLINK                         */
    public const byte NCBSENDNA = 0x71;         /* NCB SEND NO ACK                    */
    public const byte NCBCHAINSENDNA = 0x72;    /* NCB CHAIN SEND NO ACK              */
    public const byte NCBLANSTALERT = 0x73;     /* NCB LAN STATUS ALERT               */
    public const byte NCBACTION = 0x77;         /* NCB ACTION                         */
    public const byte NCBFINDNAME = 0x78;       /* NCB FIND NAME                      */
    public const byte NCBTRACE = 0x79;          /* NCB TRACE                          */
    public const byte ASYNCH = 0x80;            /* high bit set == asynchronous       */

    //коды ошибок
    public const byte NRC_GOODRET = 0x00;   /* good return                                */
                                            /* also returned when ASYNCH request accepted */
    public const byte NRC_BUFLEN = 0x01;    /* illegal buffer length                      */
    public const byte NRC_ILLCMD = 0x03;    /* illegal command                            */
    public const byte NRC_CMDTMO = 0x05;    /* command timed out                          */
    public const byte NRC_INCOMP = 0x06;    /* message incomplete, issue another command  */
    public const byte NRC_BADDR = 0x07;     /* illegal buffer address                     */
    public const byte NRC_SNUMOUT = 0x08;   /* session number out of range                */
    public const byte NRC_NORES = 0x09;     /* no resource available                      */
    public const byte NRC_SCLOSED = 0x0a;   /* session closed                             */
    public const byte NRC_CMDCAN = 0x0b;    /* command cancelled                          */
    public const byte NRC_DUPNAME = 0x0d;   /* duplicate name                             */
    public const byte NRC_NAMTFUL = 0x0e;   /* name table full                            */
    public const byte NRC_ACTSES = 0x0f;    /* no deletions, name has active sessions     */
    public const byte NRC_LOCTFUL = 0x11;   /* local session table full                   */
    public const byte NRC_REMTFUL = 0x12;   /* remote session table full                  */
    public const byte NRC_ILLNN = 0x13;     /* illegal name number                        */
    public const byte NRC_NOCALL = 0x14;    /* no callname                                */
    public const byte NRC_NOWILD = 0x15;    /* cannot put * in NCB_NAME                   */
    public const byte NRC_INUSE = 0x16;     /* name in use on remote adapter              */
    public const byte NRC_NAMERR = 0x17;    /* name deleted                               */
    public const byte NRC_SABORT = 0x18;    /* session ended abnormally                   */
    public const byte NRC_NAMCONF = 0x19;   /* name conflict detected                     */
    public const byte NRC_IFBUSY = 0x21;    /* interface busy, IRET before retrying       */
    public const byte NRC_TOOMANY = 0x22;   /* too many commands outstanding, retry later */
    public const byte NRC_BRIDGE = 0x23;    /* ncb_lana_num field invalid                 */
    public const byte NRC_CANOCCR = 0x24;   /* command completed while cancel occurring   */
    public const byte NRC_CANCEL = 0x26;    /* command not valid to cancel                */
    public const byte NRC_DUPENV = 0x30;    /* name defined by anther local process       */
    public const byte NRC_ENVNOTDEF = 0x34;   /* environment undefined. RESET required      */
    public const byte NRC_OSRESNOTAV = 0x35;  /* required OS resources exhausted            */
    public const byte NRC_MAXAPPS = 0x36;     /* max number of applications exceeded        */
    public const byte NRC_NOSAPS = 0x37;      /* no saps available for netbios              */
    public const byte NRC_NORESOURCES = 0x38; /* requested resources are not available      */
    public const byte NRC_INVADDRESS = 0x39;  /* invalid ncb address or length > segment    */
    public const byte NRC_INVDDID = 0x3B;     /* invalid NCB DDID                           */
    public const byte NRC_LOCKFAIL = 0x3C;    /* lock of user area failed                   */
    public const byte NRC_OPENERR = 0x3f;     /* NETBIOS not loaded                         */
    public const byte NRC_SYSTEM = 0x40;      /* system error                               */
    public const byte NRC_PENDING = 0xff;     /* asynchronous command is not yet finished   */


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NCB
    {
        public byte ncb_command;       /* command code                   */
        public byte ncb_retcode;       /* return code                    */
        public byte ncb_lsn;           /* local session number           */
        public byte ncb_num;           /* number of our network name     */
        public IntPtr ncb_buffer;      /* address of message buffer      */
        public UInt16 ncb_length;      /* size of message buffer         */

        public byte ncb_callname0; /* blank-padded name of remote    */
        public byte ncb_callname1; /* blank-padded name of remote    */
        public byte ncb_callname2; /* blank-padded name of remote    */
        public byte ncb_callname3; /* blank-padded name of remote    */
        public byte ncb_callname4; /* blank-padded name of remote    */
        public byte ncb_callname5; /* blank-padded name of remote    */
        public byte ncb_callname6; /* blank-padded name of remote    */
        public byte ncb_callname7; /* blank-padded name of remote    */
        public byte ncb_callname8; /* blank-padded name of remote    */
        public byte ncb_callname9; /* blank-padded name of remote    */
        public byte ncb_callname10; /* blank-padded name of remote    */
        public byte ncb_callname11; /* blank-padded name of remote    */
        public byte ncb_callname12; /* blank-padded name of remote    */
        public byte ncb_callname13; /* blank-padded name of remote    */
        public byte ncb_callname14; /* blank-padded name of remote    */
        public byte ncb_callname15; /* blank-padded name of remote    */

        public byte ncb_name0;     /* our blank-padded netname       */
        public byte ncb_name1;     /* our blank-padded netname       */
        public byte ncb_name2;     /* our blank-padded netname       */
        public byte ncb_name3;     /* our blank-padded netname       */
        public byte ncb_name4;     /* our blank-padded netname       */
        public byte ncb_name5;     /* our blank-padded netname       */
        public byte ncb_name6;     /* our blank-padded netname       */
        public byte ncb_name7;     /* our blank-padded netname       */
        public byte ncb_name8;     /* our blank-padded netname       */
        public byte ncb_name9;     /* our blank-padded netname       */
        public byte ncb_name10;    /* our blank-padded netname       */
        public byte ncb_name11;    /* our blank-padded netname       */
        public byte ncb_name12;    /* our blank-padded netname       */
        public byte ncb_name13;    /* our blank-padded netname       */
        public byte ncb_name14;    /* our blank-padded netname       */
        public byte ncb_name15;    /* our blank-padded netname       */

        public byte ncb_rto;             /* rcv timeout/retry count        */
        public byte ncb_sto;             /* send timeout/sys timeout       */
        public IntPtr ncb_post;          /* POST routine address        */
        public byte ncb_lana_num;        /* lana (adapter) number          */
        public byte ncb_cmd_cplt;        /* 0xff => commmand pending       */
        public byte ncb_reserve1;        /* reserved, used by BIOS         */
        public byte ncb_reserve2;        /* reserved, used by BIOS         */
        public byte ncb_reserve3;        /* reserved, used by BIOS         */
        public byte ncb_reserve4;        /* reserved, used by BIOS         */
        public byte ncb_reserve5;        /* reserved, used by BIOS         */
        public byte ncb_reserve6;        /* reserved, used by BIOS         */
        public byte ncb_reserve7;        /* reserved, used by BIOS         */
        public byte ncb_reserve8;        /* reserved, used by BIOS         */
        public byte ncb_reserve9;        /* reserved, used by BIOS         */
        public byte ncb_reserve10;       /* reserved, used by BIOS         */
        public IntPtr ncb_event;         /* HANDLE to Win32 event which    */

        public void Set_ncb_name(String text)
        {
            if (text.Length < 16)
            {
                text = text.PadRight(16, ' ');
            }
            else
            {
                text = text.Substring(0, 16);
            }
            byte[] buf = Encoding.ASCII.GetBytes(text);
            ncb_name0 = buf[0];
            ncb_name1 = buf[1];
            ncb_name2 = buf[2];
            ncb_name3 = buf[3];
            ncb_name4 = buf[4];
            ncb_name5 = buf[5];
            ncb_name6 = buf[6];
            ncb_name7 = buf[7];
            ncb_name8 = buf[8];
            ncb_name9 = buf[9];
            ncb_name10 = buf[10];
            ncb_name11 = buf[11];
            ncb_name12 = buf[12];
            ncb_name13 = buf[13];
            ncb_name14 = buf[14];
            ncb_name15 = buf[15];
        }

        public void Set_ncb_callname(String text)
        {
            if (text.Length < 16)
            {
                text = text.PadRight(16, ' ');
            }
            else
            {
                text = text.Substring(0, 16);
            }
            byte[] buf = Encoding.ASCII.GetBytes(text);
            ncb_callname0 = buf[0];
            ncb_callname1 = buf[1];
            ncb_callname2 = buf[2];
            ncb_callname3 = buf[3];
            ncb_callname4 = buf[4];
            ncb_callname5 = buf[5];
            ncb_callname6 = buf[6];
            ncb_callname7 = buf[7];
            ncb_callname8 = buf[8];
            ncb_callname9 = buf[9];
            ncb_callname10 = buf[10];
            ncb_callname11 = buf[11];
            ncb_callname12 = buf[12];
            ncb_callname13 = buf[13];
            ncb_callname14 = buf[14];
            ncb_callname15 = buf[15];
        }
    }


    //вызов API-функции NetBios(PNCB ncb)
    public static byte NetBiosCall(ref NCB pNCB)
    {
        byte[] buf = new byte[131072];
        IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buf, 0);
        Marshal.StructureToPtr(pNCB, ptr, false);

        Netbios(ptr);

        return pNCB.ncb_retcode;
    }


    //получение MAC-адреса по имени хоста
    public static String GetMacAddressByHostname(String RemoteHostName)
    {
        //Попытка подключения на удаленный порт 139,
        //для косвенного определения поддержки NetBIOS
        try
        {
            System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
            client.Connect(RemoteHostName, 139);
            client.Close();
        }
        catch (Exception ex)
        {
            return "";
        }
        
        //Сброс NetBIOS для адаптера 0
        NCB SNcb = new NCB();
        SNcb.ncb_command = NCBRESET;
        SNcb.ncb_lana_num = 5;
        SNcb.ncb_sto = 1;
        SNcb.ncb_rto = 1;
        byte rc = NetBiosCall(ref SNcb);
        if (rc != NRC_GOODRET)
            throw new Exception("Ошибка NetBIOS: код ошибки 0x" + rc.ToString("X2"));

        //получение списка кодов сетевых адаптеров
        //нулевой элемент - размер списка
        byte[] LANA_ENUM = new byte[256];
        IntPtr pLANA_ENUM = Marshal.UnsafeAddrOfPinnedArrayElement(LANA_ENUM, 0);
        SNcb.ncb_command = NCBENUM;
        SNcb.ncb_sto = 1;
        SNcb.ncb_rto = 1;
        SNcb.ncb_buffer = pLANA_ENUM;
        SNcb.ncb_length = 256;
        rc = NetBiosCall(ref SNcb);
        if (rc != NRC_GOODRET)
            throw new Exception("Ошибка NetBIOS: код ошибки 0x" + rc.ToString("X2"));

        //получение массива кодов адаптеров с сортировкой
        byte[] LanAdaptersList = new byte[LANA_ENUM[0]];
        for (int i = 1; i <= LANA_ENUM[0]; i++)
        {
            LanAdaptersList[i - 1] = LANA_ENUM[i];
        }
        Array.Sort(LanAdaptersList);

        //попытка получения MAC-адреса удаленного хоста из подсети каждого из адаптеров
        for (byte i = 0; i < LanAdaptersList.Length; i++)
        {
            //Сброс NetBIOS для адаптера                
            SNcb.ncb_command = NCBRESET;
            SNcb.ncb_lana_num = LanAdaptersList[i];
            SNcb.ncb_sto = 1;
            SNcb.ncb_rto = 1;
            rc = NetBiosCall(ref SNcb);
            if (rc != NRC_GOODRET)
                throw new Exception("Ошибка NetBIOS: код ошибки 0x" + rc.ToString("X2"));

            //получение MAC-адреса по имени хоста
            byte[] Status = new byte[256];
            SNcb.ncb_sto = 1;
            SNcb.ncb_rto = 1;
            SNcb.ncb_command = NCBASTAT;
            IntPtr pStatus = Marshal.UnsafeAddrOfPinnedArrayElement(Status, 0);
            SNcb.ncb_buffer = pStatus;
            SNcb.ncb_length = 256;
            SNcb.Set_ncb_callname(RemoteHostName);
            SNcb.ncb_lana_num = LanAdaptersList[i];
            SNcb.ncb_command = NCBASTAT;
            rc = NetBiosCall(ref SNcb);
            if (rc == NRC_GOODRET)
            {
                String ret = "";
                for (int c = 0; c < 6; c++)
                    ret += Status[c].ToString("X2") + " ";
                ret = ret.Trim().Replace(" ", "-");
                if (ret != "00-00-00-00-00-00") return ret;
            }
        }
        throw new Exception("Не удается определить MAC-адрес удаленного компьютера" + rc.ToString());
    }

}
