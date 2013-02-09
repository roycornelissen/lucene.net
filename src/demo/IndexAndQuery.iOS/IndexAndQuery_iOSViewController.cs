using System;
using System.Drawing;

using System.IO;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Version = Lucene.Net.Util.Version;

namespace IndexAndQuery.iOS
{
	public partial class IndexAndQuery_iOSViewController : UIViewController
	{
		public IndexAndQuery_iOSViewController () : base ("IndexAndQuery_iOSViewController", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}

		partial void StartIndexing (NSObject sender)
		{
			var appPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			DirectoryInfo indexDirectoryInfo = new DirectoryInfo(Path.Combine(appPath, "Search"));

			using (var directory = new SimpleFSDirectory(indexDirectoryInfo))
			{
				var standardAnalyzer = new StandardAnalyzer(Version.LUCENE_30);
				
				using (var writer = new IndexWriter(directory, standardAnalyzer,IndexWriter.MaxFieldLength.UNLIMITED))
				{
					Document doc = new Document();
					doc.Add(new Field("id","1",Field.Store.YES, Field.Index.ANALYZED));
					doc.Add(new Field("title","The quick brown fox jumps over the lazy dog.",Field.Store.YES, Field.Index.ANALYZED));
					
					writer.AddDocument(doc);
					writer.Optimize(true);
					writer.Flush(true,true,true);
				}
				
				var query = new BooleanQuery();
				query.Add(new TermQuery(new Term("title","fox")),Occur.MUST);
				
				IndexSearcher searcher = new IndexSearcher(directory);
				var results = searcher.Search(query, Int16.MaxValue);
				
				using (var reader = IndexReader.Open(directory, true))
				{
					foreach (var result in results.ScoreDocs)
					{
						var doc = reader.Document(result.Doc);
						
						Console.WriteLine("found document: {0}",doc.Get("id"));
					}
				}
			}
		}
	}
}

