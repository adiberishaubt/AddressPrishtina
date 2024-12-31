import React from 'react';
import { useToken } from '../TokenContext';
import { Link } from 'react-router-dom';

const AddressTable = ({ addresses, action, handleAction }) => {
    const { token } = useToken();
    const userId = localStorage.getItem("userId");

    return (
        <div className="overflow-x-auto">
            <table className="min-w-full bg-white border border-gray-200">
                <thead className="bg-gray-800 text-white">
                    <tr>
                        <th className="py-2 px-4 text-left">Street (Rruga)</th>
                        <th className="py-2 px-4 text-left">Number (Numri)</th>
                        <th className="py-2 px-4 text-left">Creator Name</th>
                        <th className="py-2 px-4 text-center">Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {addresses.length > 0 ? (
                        addresses.map((a, index) => (
                            <tr key={index} className="hover:bg-gray-100">
                                <td className="py-2 px-4 border-t">{a.rruga}</td>
                                <td className="py-2 px-4 border-t">{a.numri}</td>
                                <td className="py-2 px-4 border-t">{a.creatorName}</td>
                                <td className="py-2 px-4 border-t text-center">
                                    {
                                        action == "Delete" &&
                                        <Link to={`/address/edit/${a.id}`}>
                                            <button
                                                className={`bg-blue-600 hover:bg-blue-700 focus:ring-blue-500' mr-4 text-white px-3 py-1 rounded-md focus:outline-none focus:ring-2`}
                                                disabled={token && userId === a.creatorId ? false : true}
                                            >
                                                Update
                                            </button>
                                        </Link>
                                    }
                                    <button
                                        onClick={() => handleAction(a.id)}
                                        className={`${action === "Delete" ? 'bg-red-600 hover:bg-red-700 focus:ring-red-500' : 'bg-green-600 hover:bg-green-700 focus:ring-green-500'} text-white px-3 py-1 rounded-md focus:outline-none focus:ring-2`}
                                        disabled={action === "Approve" || (token && userId === a.creatorId) ? false : true}
                                    >
                                        {action}
                                    </button>
                                </td>
                            </tr>
                        ))
                    ) : (
                        <tr>
                            <td colSpan="4" className="py-4 px-4 text-center">
                                No addresses available.
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    );
};

export default AddressTable;
