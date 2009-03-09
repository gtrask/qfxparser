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

namespace QFXParser
{
    public class Token
    {
        public enum TokType { OPEN_TAG, TEXT, CLOSE_TAG, EOF };
        TokType m_type;
        string m_text;
        string m_value;
        int m_pos;
        int m_line;
        int m_col;

        public Token(TokType type, string text, string value, int pos, int line, int col)
        {
            m_type = type;
            m_value = value;
            m_text = text;
            m_pos = pos;
            m_line = line;
            m_col = col;
        }

        public int Pos
        {
            get
            {
                return m_pos;
            }
        }
        public int Line
        {
            get
            {
                return m_line;
            }
        }
        public int Col
        {
            get
            {
                return m_col;
            }
        }

        public string Text
        {
            get
            {
                return m_text;
            }
        }

        public TokType Type {
            get
            {
                return m_type;
            }
        }

        public string Value
        {
            get
            {
                return m_value;
            }
        }
    }
}
