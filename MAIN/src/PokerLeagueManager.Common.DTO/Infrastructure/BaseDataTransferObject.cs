using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PokerLeagueManager.Common.DTO.Infrastructure
{
    [DataContract]
    public class BaseDataTransferObject : IDataTransferObject
    {
        public BaseDataTransferObject()
        {
            DtoId = Guid.NewGuid();
        }

        [DataMember]
        [Key]
        public Guid DtoId { get; set; }
    }
}
