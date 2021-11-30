using System;

namespace MasterDetailTemplate.Util {
    /// <summary>
    /// 不要进行此调用异常。
    /// </summary>
    public class DoNotCallThisExcpetion : Exception {
        /// <summary>
        /// 不要进行此调用异常。
        /// </summary>
        public DoNotCallThisExcpetion() : base("不应该调用此项目。") {
        }
    }
}