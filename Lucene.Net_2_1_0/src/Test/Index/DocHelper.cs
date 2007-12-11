/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;

using Analyzer = Lucene.Net.Analysis.Analyzer;
using WhitespaceAnalyzer = Lucene.Net.Analysis.WhitespaceAnalyzer;
using Document = Lucene.Net.Documents.Document;
using Field = Lucene.Net.Documents.Field;
using Fieldable = Lucene.Net.Documents.Fieldable;
using Similarity = Lucene.Net.Search.Similarity;
using Directory = Lucene.Net.Store.Directory;

namespace Lucene.Net.Index
{
	
	class DocHelper
	{
		public const System.String FIELD_1_TEXT = "field one text";
		public const System.String TEXT_FIELD_1_KEY = "textField1";
		public static Field textField1;
		
		public const System.String FIELD_2_TEXT = "field field field two text";
		//Fields will be lexicographically sorted.  So, the order is: field, text, two
		public static readonly int[] FIELD_2_FREQS = new int[]{3, 1, 1};
		public const System.String TEXT_FIELD_2_KEY = "textField2";
		public static Field textField2;
		
        public const System.String FIELD_2_COMPRESSED_TEXT = "field field field two text";
        //Fields will be lexicographically sorted.  So, the order is: field, text, two
        public static readonly int[] COMPRESSED_FIELD_2_FREQS = new int[]{3, 1, 1};
        public const System.String COMPRESSED_TEXT_FIELD_2_KEY = "compressedTextField2";
        public static Field compressedTextField2;


        public const System.String FIELD_3_TEXT = "aaaNoNorms aaaNoNorms bbbNoNorms";
		public const System.String TEXT_FIELD_3_KEY = "textField3";
		public static Field textField3;
		
		public const System.String KEYWORD_TEXT = "Keyword";
		public const System.String KEYWORD_FIELD_KEY = "keyField";
		public static Field keyField;
		
		public const System.String NO_NORMS_TEXT = "omitNormsText";
		public const System.String NO_NORMS_KEY = "omitNorms";
		public static Field noNormsField;
		
		public const System.String UNINDEXED_FIELD_TEXT = "unindexed field text";
		public const System.String UNINDEXED_FIELD_KEY = "unIndField";
		public static Field unIndField;
		
		
		public const System.String UNSTORED_1_FIELD_TEXT = "unstored field text";
		public const System.String UNSTORED_FIELD_1_KEY = "unStoredField1";
		public static Field unStoredField1;
		
		public const System.String UNSTORED_2_FIELD_TEXT = "unstored field text";
		public const System.String UNSTORED_FIELD_2_KEY = "unStoredField2";
		public static Field unStoredField2;
		
        public const System.String LAZY_FIELD_BINARY_KEY = "lazyFieldBinary";
        public static byte[] LAZY_FIELD_BINARY_BYTES;
        public static Field lazyFieldBinary;
		
        public const System.String LAZY_FIELD_KEY = "lazyField";
        public const System.String LAZY_FIELD_TEXT = "These are some field bytes";
        public static Field lazyField;
		
        public const System.String LARGE_LAZY_FIELD_KEY = "largeLazyField";
        public static System.String LARGE_LAZY_FIELD_TEXT;
        public static Field largeLazyField;
		
        //From Issue 509
        public const System.String FIELD_UTF1_TEXT = "field one \u4e00text";
        public const System.String TEXT_FIELD_UTF1_KEY = "textField1Utf8";
        public static Field textUtfField1;
		
        public const System.String FIELD_UTF2_TEXT = "field field field \u4e00two text";
        //Fields will be lexicographically sorted.  So, the order is: field, text, two
        public static readonly int[] FIELD_UTF2_FREQS = new int[]{3, 1, 1};
        public const System.String TEXT_FIELD_UTF2_KEY = "textField2Utf8";
        public static Field textUtfField2;
		
		
		
		
        public static System.Collections.IDictionary nameValues = null;
		
		// ordered list of all the fields...
		// could use LinkedHashMap for this purpose if Java1.4 is OK
        public static Field[] fields = new Field[]{textField1, textField2, textField3, compressedTextField2, keyField, noNormsField, unIndField, unStoredField1, unStoredField2, textUtfField1, textUtfField2, lazyField, lazyFieldBinary, largeLazyField};
		
		// Map<String fieldName, Field field>
		public static System.Collections.IDictionary all = new System.Collections.Hashtable();
		public static System.Collections.IDictionary indexed = new System.Collections.Hashtable();
		public static System.Collections.IDictionary stored = new System.Collections.Hashtable();
		public static System.Collections.IDictionary unstored = new System.Collections.Hashtable();
		public static System.Collections.IDictionary unindexed = new System.Collections.Hashtable();
		public static System.Collections.IDictionary termvector = new System.Collections.Hashtable();
		public static System.Collections.IDictionary notermvector = new System.Collections.Hashtable();
		public static System.Collections.IDictionary lazy = new System.Collections.Hashtable();
		public static System.Collections.IDictionary noNorms = new System.Collections.Hashtable();
		
		
		private static void  Add(System.Collections.IDictionary map, Fieldable field)
		{
			map[field.Name()] = field;
		}
		
		/// <summary> Adds the fields above to a document </summary>
		/// <param name="doc">The document to write
		/// </param>
		public static void  SetupDoc(Lucene.Net.Documents.Document doc)
		{
			for (int i = 0; i < fields.Length; i++)
			{
				doc.Add(fields[i]);
			}
		}
		
		/// <summary> Writes the document to the directory using a segment named "test"</summary>
		/// <param name="dir">
		/// </param>
		/// <param name="doc">
		/// </param>
		/// <throws>  IOException </throws>
		public static void  WriteDoc(Directory dir, Lucene.Net.Documents.Document doc)
		{
			WriteDoc(dir, "test", doc);
		}
		
		/// <summary> Writes the document to the directory in the given segment</summary>
		/// <param name="dir">
		/// </param>
		/// <param name="segment">
		/// </param>
		/// <param name="doc">
		/// </param>
		/// <throws>  IOException </throws>
		public static void  WriteDoc(Directory dir, System.String segment, Lucene.Net.Documents.Document doc)
		{
			Similarity similarity = Similarity.GetDefault();
			WriteDoc(dir, new WhitespaceAnalyzer(), similarity, segment, doc);
		}
		
		/// <summary> Writes the document to the directory segment named "test" using the specified analyzer and similarity</summary>
		/// <param name="dir">
		/// </param>
		/// <param name="analyzer">
		/// </param>
		/// <param name="similarity">
		/// </param>
		/// <param name="doc">
		/// </param>
		/// <throws>  IOException </throws>
		public static void  WriteDoc(Directory dir, Analyzer analyzer, Similarity similarity, Lucene.Net.Documents.Document doc)
		{
			WriteDoc(dir, analyzer, similarity, "test", doc);
		}
		
		/// <summary> Writes the document to the directory segment using the analyzer and the similarity score</summary>
		/// <param name="dir">
		/// </param>
		/// <param name="analyzer">
		/// </param>
		/// <param name="similarity">
		/// </param>
		/// <param name="segment">
		/// </param>
		/// <param name="doc">
		/// </param>
		/// <throws>  IOException </throws>
		public static void  WriteDoc(Directory dir, Analyzer analyzer, Similarity similarity, System.String segment, Lucene.Net.Documents.Document doc)
		{
			DocumentWriter writer = new DocumentWriter(dir, analyzer, similarity, 50);
			writer.AddDocument(segment, doc);
		}
		
		public static int NumFields(Lucene.Net.Documents.Document doc)
		{
			return doc.GetFields().Count;
		}

            /*
0        textField1, 
1        textField2, 
2        textField3, 
3        compressedTextField2, 
4        keyField, 
5        noNormsField, 
6        unIndField, 
7        unStoredField1, 
8        unStoredField2, 
9        textUtfField1, 
10       textUtfField2, 
11       lazyField, 
12       lazyFieldBinary, 
13       largeLazyField
            */

        static DocHelper()
		{
            textField1 = new Field(TEXT_FIELD_1_KEY, FIELD_1_TEXT, Field.Store.YES, Field.Index.TOKENIZED, Field.TermVector.NO);
            fields[0] = textField1;
            textField2 = new Field(TEXT_FIELD_2_KEY, FIELD_2_TEXT, Field.Store.YES, Field.Index.TOKENIZED, Field.TermVector.WITH_POSITIONS_OFFSETS);
            fields[1] = textField2;
            textField3 = new Field(TEXT_FIELD_3_KEY, FIELD_3_TEXT, Field.Store.YES, Field.Index.TOKENIZED);
            fields[2] = textField3;
            {
                textField3.SetOmitNorms(true);
            }
            compressedTextField2 = new Field(COMPRESSED_TEXT_FIELD_2_KEY, FIELD_2_COMPRESSED_TEXT, Field.Store.COMPRESS, Field.Index.TOKENIZED, Field.TermVector.WITH_POSITIONS_OFFSETS);
            fields[3] = compressedTextField2;
            keyField = new Field(KEYWORD_FIELD_KEY, KEYWORD_TEXT, Field.Store.YES, Field.Index.UN_TOKENIZED);
            fields[4] = keyField;
            noNormsField = new Field(NO_NORMS_KEY, NO_NORMS_TEXT, Field.Store.YES, Field.Index.NO_NORMS);
            fields[5] = noNormsField;
            unIndField = new Field(UNINDEXED_FIELD_KEY, UNINDEXED_FIELD_TEXT, Field.Store.YES, Field.Index.NO);
            fields[6] = unIndField;
            unStoredField1 = new Field(UNSTORED_FIELD_1_KEY, UNSTORED_1_FIELD_TEXT, Field.Store.NO, Field.Index.TOKENIZED, Field.TermVector.NO);
            fields[7] = unStoredField1;
            unStoredField2 = new Field(UNSTORED_FIELD_2_KEY, UNSTORED_2_FIELD_TEXT, Field.Store.NO, Field.Index.TOKENIZED, Field.TermVector.YES);
            fields[8] = unStoredField2;
            textUtfField1 = new Field(TEXT_FIELD_UTF1_KEY, FIELD_UTF1_TEXT, Field.Store.YES, Field.Index.TOKENIZED, Field.TermVector.NO);
            fields[9] = textUtfField1;
            textUtfField2 = new Field(TEXT_FIELD_UTF2_KEY, FIELD_UTF2_TEXT, Field.Store.YES, Field.Index.TOKENIZED, Field.TermVector.WITH_POSITIONS_OFFSETS);
            fields[10] = textUtfField2;
            lazyField = new Field(LAZY_FIELD_KEY, LAZY_FIELD_TEXT, Field.Store.YES, Field.Index.TOKENIZED);
            fields[11] = lazyField;
			{
                //Initialize the large Lazy Field
                System.Text.StringBuilder buffer = new System.Text.StringBuilder();
                for (int i = 0; i < 10000; i++)
                {
                    buffer.Append("Lazily loading lengths of language in lieu of laughing ");
                }
				
                try
                {
                    LAZY_FIELD_BINARY_BYTES = System.Text.Encoding.UTF8.GetBytes("These are some binary field bytes");
                }
                catch (System.IO.IOException e)
                {
                }
                lazyFieldBinary = new Field(LAZY_FIELD_BINARY_KEY, LAZY_FIELD_BINARY_BYTES, Field.Store.YES);
                fields[fields.Length - 2] = lazyFieldBinary;
                LARGE_LAZY_FIELD_TEXT = buffer.ToString();
                largeLazyField = new Field(LARGE_LAZY_FIELD_KEY, LARGE_LAZY_FIELD_TEXT, Field.Store.YES, Field.Index.TOKENIZED);
                fields[fields.Length - 1] = largeLazyField;
				for (int i = 0; i < fields.Length; i++)
				{
					Fieldable f = fields[i];
					Add(all, f);
					if (f.IsIndexed())
						Add(indexed, f);
					else
						Add(unindexed, f);
					if (f.IsTermVectorStored())
						Add(termvector, f);
					if (f.IsIndexed() && !f.IsTermVectorStored())
						Add(notermvector, f);
					if (f.IsStored())
						Add(stored, f);
					else
						Add(unstored, f);
					if (f.GetOmitNorms())
						Add(noNorms, f);
                    if (f.IsLazy())
                        Add(lazy, f);
                }
			}
			{
                nameValues = new System.Collections.Hashtable();
                nameValues[TEXT_FIELD_1_KEY] = FIELD_1_TEXT;
                nameValues[TEXT_FIELD_2_KEY] = FIELD_2_TEXT;
                nameValues[COMPRESSED_TEXT_FIELD_2_KEY] = FIELD_2_COMPRESSED_TEXT;
                nameValues[TEXT_FIELD_3_KEY] = FIELD_3_TEXT;
                nameValues[KEYWORD_FIELD_KEY] = KEYWORD_TEXT;
                nameValues[NO_NORMS_KEY] = NO_NORMS_TEXT;
                nameValues[UNINDEXED_FIELD_KEY] = UNINDEXED_FIELD_TEXT;
                nameValues[UNSTORED_FIELD_1_KEY] = UNSTORED_1_FIELD_TEXT;
                nameValues[UNSTORED_FIELD_2_KEY] = UNSTORED_2_FIELD_TEXT;
                nameValues[LAZY_FIELD_KEY] = LAZY_FIELD_TEXT;
                nameValues[LAZY_FIELD_BINARY_KEY] = LAZY_FIELD_BINARY_BYTES;
                nameValues[LARGE_LAZY_FIELD_KEY] = LARGE_LAZY_FIELD_TEXT;
                nameValues[TEXT_FIELD_UTF1_KEY] = FIELD_UTF1_TEXT;
                nameValues[TEXT_FIELD_UTF2_KEY] = FIELD_UTF2_TEXT;
			}
		}
	}
}