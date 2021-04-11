using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor.Expressions;
using UnityEngine;

namespace AllTrickOverView.Core
{
	// Token: 0x020001A7 RID: 423
	internal class SyntaxHighlighter
	{
		// Token: 0x06000B3E RID: 2878 RVA: 0x00035ADE File Offset: 0x00033CDE
		public static string Parse(string text)
		{
			return new SyntaxHighlighter().ParseText(text);
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00035AEC File Offset: 0x00033CEC
		public string ParseText(string text)
		{
			this.result.Length = 0;
			this.statement.Clear();
			this.textPosition = 0;
			this.tokenizer = new Tokenizer(text)
			{
				TokenizeComments = true,
				TokenizePreprocessors = true
			};
			this.ReadDeclaration();
			return this.result.ToString();
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x00035B44 File Offset: 0x00033D44
		private void ReadDeclaration()
		{
			Token token = Token.UNKNOWN;
			while (token != Token.EOF)
			{
				token = this.tokenizer.GetNextToken();
				if (token == Token.EOF)
				{
					break;
				}
				if (token == Token.COMMENT)
				{
					this.AppendWhitespace(this.tokenizer.TokenStartedStringPosition);
					this.Colorize(this.tokenizer.TokenStartedStringPosition, this.tokenizer.ExpressionStringPosition - this.tokenizer.TokenStartedStringPosition, SyntaxHighlighter.CommentColor);
				}
				else if (token == Token.PREPROCESSOR)
				{
					this.AppendWhitespace(this.tokenizer.TokenStartedStringPosition);
					this.Append(this.tokenizer.TokenStartedStringPosition, this.tokenizer.ExpressionStringPosition - this.tokenizer.TokenStartedStringPosition);
				}
				else
				{
					this.statement.Add(new SyntaxHighlighter.TokenBuffer(token, this.tokenizer));
					if (this.statement[0].Token == Token.LEFT_BRACKET)
					{
						if (token == Token.RIGHT_BRACKET)
						{
							this.AppendDeclaration(this.statement, ref this.textPosition);
							this.statement.Clear();
						}
					}
					else if (token == Token.SCOPE_BEGIN)
					{
						if (this.statement.Any((SyntaxHighlighter.TokenBuffer i) => i.Token == Token.LEFT_PARENTHESIS))
						{
							this.AppendMember(this.statement);
							this.statement.Clear();
							this.ReadImplementation();
						}
						else if (this.statement.Any((SyntaxHighlighter.TokenBuffer i) => i.Token == Token.CLASS || i.Token == Token.STRUCT || i.Token == Token.INTERFACE))
						{
							this.AppendDeclaration(this.statement, ref this.textPosition);
							this.statement.Clear();
						}
						else
						{
							this.AppendMember(this.statement);
							this.statement.Clear();
							this.ReadImplementation();
						}
					}
					else if (token == Token.SCOPE_END)
					{
						this.AppendMember(this.statement);
						this.statement.Clear();
					}
					else if (token == Token.SEMI_COLON)
					{
						this.AppendMember(this.statement);
						this.statement.Clear();
					}
				}
			}
			if (this.statement.Count > 0)
			{
				this.AppendDeclaration(this.statement, ref this.textPosition);
				this.statement.Clear();
			}
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x00035D7C File Offset: 0x00033F7C
		private void ReadImplementation()
		{
			Token token = Token.UNKNOWN;
			while (token != Token.EOF && token != Token.SCOPE_END)
			{
				token = this.tokenizer.GetNextToken();
				this.statement.Add(new SyntaxHighlighter.TokenBuffer(token, this.tokenizer));
			}
			this.AppendImplementation(this.statement);
			this.statement.Clear();
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x00035DD4 File Offset: 0x00033FD4
		private void AppendDeclaration(List<SyntaxHighlighter.TokenBuffer> statementBuffer, ref int prevIndex)
		{
			for (int i = 0; i < statementBuffer.Count; i++)
			{
				SyntaxHighlighter.TokenBuffer tokenBuffer = statementBuffer[i];
				this.AppendWhitespace(tokenBuffer.StartIndex);
				if (tokenBuffer.Token == Token.IDENTIFIER)
				{
					string @string = tokenBuffer.GetString(this.tokenizer);
					if (TypeExtensions.IsCSharpKeyword(@string))
					{
						this.Colorize(@string, SyntaxHighlighter.KeywordColor);
					}
					else
					{
						this.Colorize(@string, SyntaxHighlighter.IdentifierColor);
					}
					prevIndex = tokenBuffer.EndIndex;
				}
				else
				{
					this.AppendToken(tokenBuffer);
				}
			}
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x00035E50 File Offset: 0x00034050
		private void AppendMember(List<SyntaxHighlighter.TokenBuffer> statement)
		{
			for (int i = 0; i < statement.Count; i++)
			{
				SyntaxHighlighter.TokenBuffer tokenBuffer = statement[i];
				this.AppendWhitespace(tokenBuffer.StartIndex);
				if (tokenBuffer.Token == Token.IDENTIFIER)
				{
					string @string = tokenBuffer.GetString(this.tokenizer);
					if (TypeExtensions.IsCSharpKeyword(@string))
					{
						this.Colorize(@string, SyntaxHighlighter.KeywordColor);
					}
					else
					{
						Token token = (i + 1 < statement.Count) ? statement[i + 1].Token : Token.UNKNOWN;
						if (token != Token.UNKNOWN && (token == Token.SIMPLE_ASSIGNMENT || token == Token.SEMI_COLON || token == Token.SCOPE_BEGIN || token == Token.LEFT_PARENTHESIS || token == Token.COMMA))
						{
							this.Append(@string);
						}
						else
						{
							this.Colorize(@string, SyntaxHighlighter.IdentifierColor);
						}
					}
					this.textPosition = tokenBuffer.EndIndex;
				}
				else
				{
					this.AppendToken(tokenBuffer);
				}
			}
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x00035F1C File Offset: 0x0003411C
		private void AppendImplementation(List<SyntaxHighlighter.TokenBuffer> statement)
		{
			for (int i = 0; i < statement.Count; i++)
			{
				SyntaxHighlighter.TokenBuffer tokenBuffer = statement[i];
				this.AppendWhitespace(tokenBuffer.StartIndex);
				if (tokenBuffer.Token == Token.IDENTIFIER)
				{
					string @string = tokenBuffer.GetString(this.tokenizer);
					if (TypeExtensions.IsCSharpKeyword(@string))
					{
						this.Colorize(@string, SyntaxHighlighter.KeywordColor);
					}
					else
					{
						this.result.Append(@string);
					}
					this.textPosition = tokenBuffer.EndIndex;
				}
				else
				{
					this.AppendToken(tokenBuffer);
				}
			}
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x00035FA0 File Offset: 0x000341A0
		private void AppendToken(SyntaxHighlighter.TokenBuffer buffer)
		{
			this.AppendWhitespace(buffer.StartIndex);
			Token token = buffer.Token;
			switch (token)
			{
			case Token.SIGNED_INT32:
			case Token.UNSIGNED_INT32:
			case Token.SIGNED_INT64:
			case Token.UNSIGNED_INT64:
			case Token.FLOAT32:
			case Token.FLOAT64:
			case Token.DECIMAL:
				this.Colorize(buffer.StartIndex, buffer.Length, SyntaxHighlighter.LiteralColor);
				goto IL_1BE;
			case Token.IDENTIFIER:
			{
				string text = this.tokenizer.ExpressionString.Substring(buffer.StartIndex, buffer.Length);
				if (TypeExtensions.IsCSharpKeyword(text))
				{
					this.Colorize(text, SyntaxHighlighter.KeywordColor);
					goto IL_1BE;
				}
				this.Colorize(text, SyntaxHighlighter.IdentifierColor);
				goto IL_1BE;
			}
			case Token.SIZEOF:
				break;
			case Token.CHAR_CONSTANT:
			case Token.STRING_CONSTANT:
				this.Colorize(buffer.StartIndex, buffer.Length, SyntaxHighlighter.StringLiteralColor);
				goto IL_1BE;
			case Token.LOGICAL_OR:
			case Token.LOGICAL_AND:
				goto IL_19A;
			case Token.EOF:
				return;
			default:
				switch (token)
				{
				case Token.TRUE:
				case Token.FALSE:
				case Token.RELATIONAL_IS:
				case Token.RELATIONAL_AS:
				case Token.NEW:
				case Token.THIS:
				case Token.BASE:
				case Token.CHECKED:
				case Token.UNCHECKED:
				case Token.DEFAULT:
				case Token.NULL:
				case Token.TYPEOF:
				case Token.VOID:
				case Token.REF:
				case Token.OUT:
				case Token.IN:
				case Token.CLASS:
				case Token.STRUCT:
				case Token.INTERFACE:
				case Token.RETURN:
					break;
				case Token.NUMBERED_EXPRESSION_ARGUMENT:
				case Token.UNNUMBERED_EXPRESSION_ARGUMENT:
				case Token.NAMED_EXPRESSION_ARGUMENT:
				case Token.NULL_COALESCE:
				case Token.INCREMENT:
				case Token.DECREMENT:
				case Token.MEMBER_ACCESS_POINTER_DEREFERENCE:
				case Token.ELEMENT_ACCESS_NULL_CONDITIONAL:
				case Token.MEMBER_ACCESS_NULL_CONDITIONAL:
				case Token.DELEGATE:
				case Token.SCOPE_BEGIN:
				case Token.SCOPE_END:
				case Token.PREPROCESSOR:
					goto IL_19A;
				case Token.COMMENT:
					this.Colorize(buffer.StartIndex, buffer.Length, SyntaxHighlighter.CommentColor);
					goto IL_1BE;
				default:
					goto IL_19A;
				}
				break;
			}
			this.Colorize(buffer.StartIndex, buffer.Length, SyntaxHighlighter.KeywordColor);
			goto IL_1BE;
			IL_19A:
			this.result.Append(this.tokenizer.ExpressionString, buffer.StartIndex, buffer.Length);
			IL_1BE:
			this.textPosition = buffer.EndIndex;
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x00036178 File Offset: 0x00034378
		private void Colorize(string text, Color color)
		{
			this.result.Append("<color=#");
			this.result.Append(ColorUtility.ToHtmlStringRGBA(color));
			this.result.Append(">");
			this.Append(text);
			this.result.Append("</color>");
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x000361D4 File Offset: 0x000343D4
		private void Colorize(int start, int length, Color color)
		{
			this.result.Append("<color=#");
			this.result.Append(ColorUtility.ToHtmlStringRGBA(color));
			this.result.Append(">");
			this.Append(start, length);
			this.result.Append("</color>");
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x0003622E File Offset: 0x0003442E
		private void Append(int start, int length)
		{
			this.result.Append(this.tokenizer.ExpressionString, start, length);
			this.textPosition = start + length;
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x00036252 File Offset: 0x00034452
		private void Append(string text)
		{
			this.result.Append(text);
			this.textPosition += text.Length;
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x00036274 File Offset: 0x00034474
		private void AppendWhitespace(int position)
		{
			if (position - this.textPosition > 0)
			{
				this.result.Append(this.tokenizer.ExpressionString, this.textPosition, position - this.textPosition);
				this.textPosition = position;
			}
		}

		// Token: 0x040005E5 RID: 1509
		public static Color BackgroundColor = new Color(0.118f, 0.118f, 0.118f, 1f);

		// Token: 0x040005E6 RID: 1510
		public static Color TextColor = new Color(0.863f, 0.863f, 0.863f, 1f);

		// Token: 0x040005E7 RID: 1511
		public static Color KeywordColor = new Color(0.337f, 0.612f, 0.839f, 1f);

		// Token: 0x040005E8 RID: 1512
		public static Color IdentifierColor = new Color(0.306f, 0.788f, 0.69f, 1f);

		// Token: 0x040005E9 RID: 1513
		public static Color CommentColor = new Color(0.341f, 0.651f, 0.29f, 1f);

		// Token: 0x040005EA RID: 1514
		public static Color LiteralColor = new Color(0.71f, 0.808f, 0.659f, 1f);

		// Token: 0x040005EB RID: 1515
		public static Color StringLiteralColor = new Color(0.839f, 0.616f, 0.522f, 1f);

		// Token: 0x040005EC RID: 1516
		private Tokenizer tokenizer;

		// Token: 0x040005ED RID: 1517
		private StringBuilder result = new StringBuilder();

		// Token: 0x040005EE RID: 1518
		private List<SyntaxHighlighter.TokenBuffer> statement = new List<SyntaxHighlighter.TokenBuffer>();

		// Token: 0x040005EF RID: 1519
		private int textPosition;

		// Token: 0x0200038F RID: 911
		private struct TokenBuffer
		{
			// Token: 0x1700024D RID: 589
			// (get) Token: 0x060011D6 RID: 4566 RVA: 0x0005616A File Offset: 0x0005436A
			public int Length
			{
				get
				{
					return this.EndIndex - this.StartIndex;
				}
			}

			// Token: 0x060011D7 RID: 4567 RVA: 0x00056179 File Offset: 0x00054379
			public TokenBuffer(Token token, Tokenizer tokenizer)
			{
				this.Token = token;
				this.StartIndex = tokenizer.TokenStartedStringPosition;
				this.EndIndex = tokenizer.ExpressionStringPosition;
			}

			// Token: 0x060011D8 RID: 4568 RVA: 0x0005619A File Offset: 0x0005439A
			public string GetString(Tokenizer tokenizer)
			{
				return tokenizer.ExpressionString.Substring(this.StartIndex, this.Length);
			}

			// Token: 0x060011D9 RID: 4569 RVA: 0x000561B3 File Offset: 0x000543B3
			public override string ToString()
			{
				return this.Token.ToString();
			}

			// Token: 0x04000A38 RID: 2616
			public Token Token;

			// Token: 0x04000A39 RID: 2617
			public int StartIndex;

			// Token: 0x04000A3A RID: 2618
			public int EndIndex;
		}
	}
}
