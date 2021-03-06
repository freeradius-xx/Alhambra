﻿using Alhambra.Plugin.ConfigUtil;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Alhambra.Db.Plugin
{
    /// <summary>
    /// DB接続用のインスタンスを返すファクトリーです。
    /// </summary>
    public static class PluginFactory
    {
        /// <summary>
        /// プラグインを配置するディレクトリ名
        /// </summary>
        private const string PLUGINS_DIRECTORY_NMAE = "Plugins";

        /// <summary>
        /// プラグインのカタログ
        /// </summary>
        private static AggregateCatalog _catalog = new AggregateCatalog();

        /// <summary>
        /// プラグインのカタログを初期化します。
        /// </summary>
        static PluginFactory()
        {
            //自dllからプラグインの場所を取得
            string executionPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string pluginsPath = Path.Combine(executionPath, PLUGINS_DIRECTORY_NMAE);
            if (!Directory.Exists(pluginsPath))
            {
                Directory.CreateDirectory(pluginsPath);
            }

            _catalog.Catalogs.Add(new DirectoryCatalog(pluginsPath).FilterPlugin());

            //ASP.NETの場合dllのあるディレクトリをHttpRuntime.BinDirectoryから取得
            try
            {
                _catalog.Catalogs.Add(new DirectoryCatalog(Path.Combine(HttpRuntime.BinDirectory, PLUGINS_DIRECTORY_NMAE)).FilterPlugin());
            }
            catch (ArgumentNullException)
            {
                //ASP.NETでない（テスト実行やクライアントアプリの）場合は例外が挙がってここに来ます。
            }

            if (_catalog.Parts.Count() == 0)
            {
                throw new AlhambraPluginException("DBHelperプラグインdllの読み込みに失敗しました。");
            }
        }

        //指定プラグインのみを読み込みます。
        private static FilteredCatalog FilterPlugin(this DirectoryCatalog catalog)
        {
            if (Config.Value.PluginName == "")
            {
                //プラグイン名が指定されていなければ全てを返します。
                return catalog.Filter(c => true);
            }
            else
            {
                return catalog.Filter(c => c.ToString().StartsWith("Alhambra.Plugin." + Config.Value.PluginName));
            }
        }

        #region DBBridge
        /// <summary>
        /// 新規のインスタンスを返します。
        /// </summary>
        public static AbstractDBBridge NewDB
        {
            get
            {
                var i = new DBIncubator();
                try
                {
                    new CompositionContainer(_catalog).ComposeParts(i);
                }
                catch (ChangeRejectedException cre)
                {
                    throw new AlhambraPluginException("DBHelperプラグインにAbstractDBBridgeの定義がありません。", cre);
                }

                return i.Incubatee;
            }
        }

        /// <summary>
        /// Managed Extensibility FrameworkはDIフレームワークなので生成したインスタンスを直接返却できません。<br />
        /// そこで、セッターインジェクションを使い、生成したAbstractDBBridge型のインスタンスを<br />
        /// 一回このクラスのプロパティにセットしてからDBFactioryのNewDBプロパティで返却します。<br />
        /// </summary>
        public class DBIncubator
        {
            [Import(typeof(AbstractDBBridge))]
            internal AbstractDBBridge Incubatee { get; set; }
        }
        #endregion

        #region SQLDefinition
        /// <summary>
        /// DBのSQL方言を吸収するプラグインを取得します。
        /// </summary>
        public static ISqlDefinition SqlDefinition
        {
            get
            {
                var i = new SqlDifinitionIncubator();
                try
                {
                    new CompositionContainer(_catalog).ComposeParts(i);
                }
                catch
                {
                    // プラグインが無ければデフォルト定義を返します。
                    return new DefaultSqlDefinition();
                }
                return i.Incubatee;
            }
        }

        /// <summary>
        /// SQL定義クラスプラグインの受け口です。
        /// </summary>
        public class SqlDifinitionIncubator
        {
            [Import(typeof(ISqlDefinition))]
            internal ISqlDefinition Incubatee { get; set; }
        }

        private class DefaultSqlDefinition : ISqlDefinition
        {
            public string MultiBytePrefix
            {
                get { return "N"; }
            }
        }
        #endregion
    }
}
