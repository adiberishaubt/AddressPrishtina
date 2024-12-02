import React, { useEffect, useState } from 'react'
import axios from 'axios';

const Address = () => {
    const [addresses, setAddresses] = useState([]);

    useEffect(() => {
        axios.get('http://localhost:5167/v1/Address')
        .then(res => setAddresses(res.data));
    }, [])

  return (
    <div>
        {addresses.length > 0 && addresses.map(a => <p>{a.rruga} : {a.numri}</p>)}
    </div>
  )
}

export default Address