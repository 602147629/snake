using System;
using System.Collections.Generic;
public class UserLogic
{
    public static UserLogic Instance = new UserLogic();
    public string ThemeUsing { get; set; }

    private UserLogic() {
        ThemeUsing = "Pure";//应由根据服务器来设置值
    }

}
