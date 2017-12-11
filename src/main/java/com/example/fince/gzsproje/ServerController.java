package com.example.fince.gzsproje;

import java.nio.charset.Charset;
import java.util.Date;

/**
 * Created by fince on 26.11.2017.
 */

public class ServerController {

    /*  si = sendinginformation*
    /
     */
    BluetoothConnectionService bcs;
    BluetoothController bc;
    MainActivity ma;
    public ServerController(BluetoothController bc,BluetoothConnectionService bcs,MainActivity ma)
    {
        this.bc=bc;
        this.bcs=bcs;
        this.ma=ma;
    }
    public void sendMessageToServer(String text)
    {
        byte[] bytes=text.getBytes(Charset.defaultCharset());
        bcs.write(bytes);
    }
    public void exit()
    {
        String nameSurname=ma.getNameSurname();
        sendMessageToServer("ex#"+nameSurname );
    }

}
