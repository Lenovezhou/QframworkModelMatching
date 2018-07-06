/****************************************************************************
 * Copyright (c) 2017 xiaojun@putao.com
 * Copyright (c) 2017 liangxie
****************************************************************************/
namespace QFramework
{
	public class QMsgSpan
	{
		public const int Count = 3000;
	}

	public partial class QMgrID
	{
		public const int Framework = 0;
		public const int UI = Framework + QMsgSpan.Count; // 3000
		public const int Audio = UI + QMsgSpan.Count; // 6000
		public const int Network_WEB = Audio + QMsgSpan.Count;
		public const int UIFilter = Network_WEB + QMsgSpan.Count;
		public const int Game = UIFilter + QMsgSpan.Count;
		public const int PCConnectMobile = Game + QMsgSpan.Count;
        public const int Camera = PCConnectMobile + QMsgSpan.Count;
        public const int Mouse = Camera + QMsgSpan.Count;
        public const int Model = Mouse + QMsgSpan.Count;
        public const int Pointer = Model + QMsgSpan.Count;
        public const int FrameworkEnded = Pointer + QMsgSpan.Count;
		public const int FrameworkMsgModuleCount = 7;
	}
}