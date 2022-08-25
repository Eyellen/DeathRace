using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServerUsernameRequirements
{
    public static string CheckIfNameIsUnique(string username, string[] otherNames)
    {
        int idx = 0;
        while (idx < otherNames.Length)
        {
            if (username == otherNames[idx])
            {
                if (CheckIfHasPostfix(otherNames[idx], out int postfix))
                {
                    int newPostfix = postfix + 1;
                    username = username.Replace(postfix.ToString(), newPostfix.ToString());
                }
                else
                {
                    username += "_1";
                }

                idx = 0;
                continue;
            }

            idx++;
        }

        return username;
    }

    /// <summary>
    /// returns postfix number
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    private static bool CheckIfHasPostfix(string username, out int postfix)
    {
        int idx = username.Length - 1;
        while (idx > 0)
        {
            if (username[idx - 1] == '_' && (username[idx] >= '0' && username[idx] <= '9')) break;

            if (idx - 1 <= 0)
            {
                postfix = default(int);
                return false;
            }

            idx--;
        }

        postfix = int.Parse(username.Substring(idx, username.Length - idx));
        return true;
    }
}
