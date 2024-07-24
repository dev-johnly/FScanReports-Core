using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Domain.Entities;

public class NGAC_AUTHLOG
{
    [Key]
    public long IndexKey { get; set; }
    public long? UserIDIndex { get; set; }
    public DateTime? TransactionTime { get; set; }
    public string UserID { get; set; }
    public int TerminalID { get; set; }
    public int? AuthType { get; set; }
    public int? AuthResult { get; set; }
    public int? FunctionKey { get; set; }
    public DateTime? ServerRecordTime { get; set; } 
    public int? Reserved { get; set; }
    public int? LogType { get; set; }
    public int? TempValue { get; set; }
    public int? MinIndex { get; set; }
    public string PairUserID { get; set; }
    public int? PairAuthType { get; set; }
    public int? PairAuthResult { get; set; }
}