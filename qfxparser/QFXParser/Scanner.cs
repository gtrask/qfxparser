/*
  QFX Parser Library - low level utility to parse Quicken QFX files into a basic
  object model.
 
  Copyright (C) 2009  Stephen P Owens

  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QFXParser
{
    class Scanner
    {
        StreamReader m_input;
        enum State { Initial, OpenTag, ClosedTag };
        State m_state;
        int m_pos = 0;
        int m_line = 0;
        int m_col = 0;
        int m_posStart = 0;
        int m_lineStart = 0;
        int m_colStart = 0;
        char m_chPrevious;
        int m_tabStop = 0;
        StringBuilder m_strBuffText;
        StringBuilder m_strBuffValue;

        public Scanner(Stream input, int tabStop)
        {
            
            m_state = State.Initial;
            m_input = new StreamReader(input);
            m_chPrevious = Convert.ToChar(0);
            m_tabStop = tabStop;
        }

        void updatePosition(char c)
        {
            switch (c)
            {
                case '\n':
                    m_line++;
                    m_col = 0;
                    m_pos++;
                    break;
                case '\t':
                    m_pos++;
                    m_col += m_tabStop;
                    break;
                default:
                    m_pos++;
                    m_col++;
                    break;
            }
        }

        void setTokBegin()
        {
            m_posStart = m_pos;
            m_lineStart = m_line;
            m_colStart = m_col;
        }

        Token procCharInitial(char c)
        {
            updatePosition(c);
            switch (c)
            {
                case '<':
                    m_strBuffText = new StringBuilder();
                    m_strBuffValue = new StringBuilder();
                    m_strBuffText.Append(c);
                    m_state = State.OpenTag;
                    setTokBegin();
                    break;
            }
            return null;
        }

        Token procCharOpenTag(char c)
        {
            updatePosition(c);
            switch (c)
            {
                case '/':
                    if (m_strBuffText.Length > 1)
                        throw new ScannerException("Unexpected '/' character.", m_pos, m_line, m_col);
                    else
                    {
                        m_strBuffText.Append(c);
                    }
                    break;
                case '<':
                    throw new ScannerException("Unexpected '<' character.", m_pos, m_line, m_col);
                    break;
                case '>':
                    m_strBuffText.Append(c);
                    m_state = State.ClosedTag;
                    Token.TokType type = Token.TokType.OPEN_TAG;
                    string text = m_strBuffText.ToString();
                    if (text[1] == '/')
                        type = Token.TokType.CLOSE_TAG;
                    Token t = new Token(type, text, m_strBuffValue.ToString(), m_posStart, m_lineStart, m_colStart);
                    m_strBuffText = new StringBuilder();
                    m_strBuffValue = new StringBuilder();
                    setTokBegin();
                    return t;                    
                default:
                    m_strBuffText.Append(c);
                    m_strBuffValue.Append(c);
                    break;
            }
            return null;
        }

        Token procCharClosedTag(char c)
        {
            Token result = null;
            updatePosition(c);
            switch (c)
            {
                case '<':
                    string text = m_strBuffText.ToString();
                    if (text.Length > 0)
                    {
                        result = new Token(Token.TokType.TEXT, m_strBuffText.ToString(), m_strBuffText.ToString(),
                            m_posStart, m_lineStart, m_colStart);
                    }
                    m_strBuffText = new StringBuilder();
                    m_strBuffText.Append(c);
                    m_strBuffValue = new StringBuilder();
                    m_state = State.OpenTag;
                    setTokBegin();
                    break;
                case '>':
                    throw new ScannerException(m_pos, m_line, m_col);                    
                default:
                    m_strBuffText.Append(c);
                    break;
            }
            return result;
        }

        public Token getNextToken()
        {
            Token result = null;
            int i = m_input.Read();
            
            while( (result == null)&& (i != -1)) 
            {
                char c = Convert.ToChar(i);
                switch (m_state)
                {
                    case State.Initial:
                        result = procCharInitial(c);
                        break;
                    case State.OpenTag:
                        result = procCharOpenTag(c);
                        break;
                    case State.ClosedTag:
                        result = procCharClosedTag(c);
                        break;                    
                }
                m_chPrevious = c;
                if(result == null)
                    i = m_input.Read();
            }
            if (i == -1)
            {
                result = new Token(Token.TokType.EOF, null, null, m_pos, m_line, m_col);
            }
            return result;
        }
    }
}
