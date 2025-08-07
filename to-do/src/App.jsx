import { useEffect, useState } from 'react'
import './App.css'
import axios from 'axios'

function App() {

  let [todo, setTodo] = useState([]);
  const [input, setInput] = useState('');

  const add = () => {
    axios.post('https://localhost:7261/api/Todo', {
      "title": input,
    }).then(res => {
      const newtodo = [...todo, res.data]
      setTodo(newtodo);
      localStorage.setItem("todo", JSON.stringify(newtodo));
      setInput("");
    }).catch(err => {
      console.log(err);
    })
  }

  const addenter = (e) => {
    if (e.key == 'Enter') {
      const newtodo = [...todo, input]
      setTodo(newtodo);
      localStorage.setItem("todo", JSON.stringify(newtodo));
      setInput("");
    }
  }

  const deletet = (i) => {
    axios.delete(`https://localhost:7261/api/Todo/${i}`, {
      headers: {
        'Content-Type': 'application/json'
      }
    }).then((res) =>{
        const newtodo = todo.filter((item) => {
          return (item.id != i)
        })
        setTodo(newtodo)
        localStorage.setItem("todo", JSON.stringify(newtodo));
      }
    )
  }

  useEffect(() => {
    // localStorage.setItem("todo ", JSON.stringify(todo));
    const temp = localStorage.getItem("todo");
    if (temp) {
      var n = JSON.parse(temp)
      setTodo(n)
    }
    if (n.length === 0 || !n) {
      axios.get('https://localhost:7261/api/Todo', {
        headers: {
          'Content-Type': 'application/json'
        }
      })
        .then((res) => {
          let a = res.data.map((item) => ({ id: item.id, todo: item.title }))
          // let a = res.data;
          // console.log(a)
          localStorage.setItem("todo", JSON.stringify(a));
          setTodo(a);
        }).catch(err => {
          console.log(err)
        })
    }
  }, [])

  return (
    <>
      <input type='text' placeholder='Type something here...' className='input-field' value={input} onKeyDown={(e) => addenter(e)} onChange={(e) => setInput(e.target.value)} />
      <button onClick={add}>Add</button>
      <div>
        {
          todo.length > 0 ? <div>
            todos:
          </div> : <div>no todo</div>
        }
      </div>
      <div>
        {
          todo.map((item, key) => {
            return (<div key={item.id}>
              {item.title}
              <button onClick={() => deletet(item.id)}>delete</button>
            </div>)
          })
        }
      </div>
    </>
  )
}

export default App
