import './Card.css'

const Card = (props) => {
    return(<div className='card-item'>
    <div className='card-icon'>
        <img src={props.icon} style={{width:50}}></img>
    </div>
    <h4 className='my-card-header'>{props.name}</h4>
    <p>{props.description}</p>
</div>)
}

export default Card;