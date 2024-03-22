using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.Networking;

public class RedeemCode : NetworkBehaviour
{
    public string Code { get; private set; }
    public string ItemToUnlock { get; private set; }
    public DateTime ExpiryDate { get; private set; }

    public RedeemCode(string code, string itemToUnlock, DateTime expiryDate)
    {
        Code = code; 
        ItemToUnlock = itemToUnlock;
        ExpiryDate = expiryDate;
    }
    
    public bool IsValid()
    {
        if (!AuthenticationService.Instance.IsSignedIn) return false;
        var task = GetUtcTimeAsync(ExpiryDate);
        task.Wait();
        return task.Result;
    }

    private static async Task<bool> GetUtcTimeAsync(DateTime expiration)
    {
        try
        {
            var client = new TcpClient();
            await client.ConnectAsync("time.nist.gov", 13);
            using var streamReader = new StreamReader(client.GetStream());
            var response = await streamReader.ReadToEndAsync();
            var utcDateTimeString = response.Substring(7, 17);
            DateTime currentTimeFromServer = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            return(currentTimeFromServer < expiration);
        }
        catch
        {
            return false;
        }
    }
}
