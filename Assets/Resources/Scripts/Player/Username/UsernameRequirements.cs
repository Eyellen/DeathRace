using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class UsernameRequirements
{
    public static bool CheckIfNameIsAppropriate(string username)
    {
        if (username == string.Empty) return false;

        Regex regex = new Regex(@"[a-z|A-Z]+_?[a-z|A-Z]+");
        Match match = regex.Match(username);

        if (match.Value == username)
            return true;

        return false;
    }
}
