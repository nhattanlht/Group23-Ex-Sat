const SelectInput = ({ options,id, name, value, required, onChange }) => {
    return (
        <select
            value={value}
            id={id}
            name={name}
            onChange={onChange}
            required={required}
            className="w-full border p-2"
        >
            <option value="">-----</option>
            {options.map((option) => (
                <option key={option.id} value={option.id}>
                    {option.name}
                </option>
            ))}
        </select>
    );
}

export default SelectInput;